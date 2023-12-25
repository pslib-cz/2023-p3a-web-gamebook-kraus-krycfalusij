using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using static lost_on_island.Models.Cards;

namespace lost_on_island.Pages.Game
{
    public class LocationModel : PageModel
    {
        private readonly ILogger<LocationModel> _logger;
        private readonly ILocationProvider _locationProvider;
        private readonly ISessionStorage<GameState> _sessionStorage;
        public GameState GameState => _sessionStorage.LoadOrCreate("GameState");
        public Location CurrentLocation { get; set; }
        public List<Connection> AvailableConnections { get; set; }
        public List<Cards.Card> LocationCards { get; set; }
        public Card SingleLocationCard { get; set; }
        Random rnd = new Random();
        public int RandomCardIndex { get; set; }

        public LocationModel(ILocationProvider locationProvider, ISessionStorage<GameState> sessionStorage)
        {
            _locationProvider = locationProvider;
            _sessionStorage = sessionStorage;
        }

        private bool IsValidTransition(GameState gameState, int targetLocationId)
        {

            // Definice ID pro speciální lokace
            const int prologId = 1;
            const int shipwreckId = 2;
            const int beachId = 3;
            const int deathId = 8;
            const int endgameId = 9;
            const int indexId = 0;


            switch (targetLocationId)
            {
                case deathId:
                    // Smrt mùže nastat kdykoliv, ale pouze pokud je hráè mrtvý
                    return gameState.IsPlayerDead;

                case endgameId:
                    // Na konec hry lze pøejít pouze pokud hra skonèila
                    return gameState.HasGameEnded;

                case prologId:
                    // Na prolog lze pøejít pouze z indexu
                    return gameState.CurrentLocationId == indexId;

                case shipwreckId:
                    // Na loï lze pøejít z prologu nebo opakovanì z pláže
                    return gameState.CurrentLocationId == prologId || gameState.CurrentLocationId == beachId;

                case indexId:
                    // Na index lze pøejít pouze na zaèátku hry
                    return gameState.CurrentLocationId == 0;

                default:
                    // Pro ostatní lokace platí standardní logika
                    return _locationProvider.IsValidConnection(gameState.CurrentLocationId, targetLocationId);
            }
        }


        private void UpdateGameState(GameState gameState, int locationId)
        {
            gameState.CurrentLocationId = locationId;
            _sessionStorage.Save("GameState", gameState);
        }

        private void LoadLocationData(int locationId, bool isRiskyMode)
        {
            CurrentLocation = _locationProvider.GetLocationById(locationId);
            AvailableConnections = _locationProvider.GetConnectionsFromLocation(locationId).ToList();
            LocationCards = new Cards().CardPacks.FirstOrDefault(pack => pack.Id == locationId)?.CardsInPack;

            int totalProbability = LocationCards.Sum(card => card.Probability);
            int randomValue = rnd.Next(1, totalProbability + 1);
            int cumulativeProbability = 0;

            if (isRiskyMode)
            {
                foreach (var card in LocationCards)
                {
                    if (card.Item == "enemy")
                    {
                        card.Probability *= 2;
                    }
                    else
                    {
                        card.ItemAdd *= 2;
                    }
                }
            }

            foreach (var card in LocationCards)
            {
                cumulativeProbability += card.Probability;
                if (randomValue <= cumulativeProbability)
                {
                    SingleLocationCard = card;
                    break;
                }
            }
        }

        public IActionResult OnPostHandleCardClick(int cardId)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            var selectedCard = GetSelectedCard(gameState.CurrentLocationId, cardId);

            if (selectedCard != null)
            {
                gameState.Turns += 1;
                if (selectedCard != null && selectedCard.Item == "enemy")
                {
                    gameState.InFight = true; // Hráè vstupuje do boje
                    _sessionStorage.Save("GameState", gameState);
                    Console.WriteLine("location id" + gameState.CurrentLocationId);
                    return RedirectToPage("/Game/Fight", new { cardPackId = (gameState.CurrentLocationId), enemyId = selectedCard.Id });
                }
                else if (selectedCard.Item == "enemy") // nešastné náhody co berou životy
                {
                    gameState.UpdateHealthAndEnergy(-10, 0);
                }
                else
                {
                    ProcessCard(selectedCard, gameState);
                }

                _sessionStorage.Save("GameState", gameState);
            }

            return RedirectToPage(new { locationId = gameState.CurrentLocationId });
        }

        private IActionResult ProcessCard(Card card, GameState gameState)
        {
            gameState.AddToInventory(card.Item, card.ItemAdd);

            if (card.Item == "food")
            {
                gameState.UpdateHealthAndEnergy(5, 0);
            }
            else if (card.Item == "enemy")
            {
                gameState.UpdateHealthAndEnergy(-10, 0); // Poškození hráèe
            }

            gameState.CheckGameProgress();
            return null; // Žádné automatické pøesmìrování
        }

        private Card GetSelectedCard(int currentLocationId, int cardId)
        {
            var rightCardPack = GameState.CurrentLocationId;
            return new Cards().CardPacks.Find(CardPack => CardPack.Id == currentLocationId).CardsInPack.FirstOrDefault(c => c.Id == cardId);
        }

        public IActionResult OnGet(int locationId)
        {

            var gameState = _sessionStorage.LoadOrCreate("GameState");

            if (!IsValidTransition(gameState, locationId))
            {
                return RedirectToPage("/Game/Cheater");
            }
            if (gameState.InFight)
            {
                return RedirectToPage("/Game/Cheater");
            }

            UpdateGameState(gameState, locationId);


            if (_locationProvider.IsSpecialLocation(locationId))
            {
                return RedirectToSpecialPage(locationId, gameState);
            }

            gameState.Turns += 1;

            _sessionStorage.Save("GameState", gameState);
            LoadLocationData(locationId, GameState.IsRiskyMode);
            return Page();
        }

        private IActionResult RedirectToSpecialPage(int locationId, GameState gameState)
        {

            switch (locationId)
            {
                
                case 1:
                    return RedirectToPage("/Game/Prolog");
                case 2:
                    return RedirectToPage("/Game/Shipwreck");
                case 8: 
                    return RedirectToPage("/Game/Death");
                case 9: 
                    return RedirectToPage("/Game/EndGame");

                default:
                    return RedirectToPage("/Game/Cheater");
            }
        }

        public IActionResult OnPostToggleRiskyMode()
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");
            gameState.IsRiskyMode = !gameState.IsRiskyMode;
            _sessionStorage.Save("GameState", gameState);

            return RedirectToPage(new { locationId = gameState.CurrentLocationId });
        }

        public IActionResult OnPostHandleBadgeClick(string badgeType)
        {
            Console.WriteLine(badgeType);
            var gameState = _sessionStorage.LoadOrCreate("GameState");
            gameState.AddTool(badgeType);
            _sessionStorage.Save("GameState", gameState);
            return RedirectToPage(new { locationId = gameState.CurrentLocationId });
        }

        public IActionResult OnPostHandleChangeLocation(string locationIdInputValue)
        {

            return RedirectToPage(new { locationId = locationIdInputValue });
        }

    }
}
