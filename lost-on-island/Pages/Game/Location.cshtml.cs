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
        public GameState GameState { get; set; }
        public Location CurrentLocation { get; set; }
        public List<Tool> Tools { get; set; }
        public List<Connection> AvailableConnections { get; set; }
        public List<Cards.Card> LocationCards { get; set; }
        public Card SingleLocationCard { get; set; }
        Random rnd = new Random();
        public int RandomCardIndex { get; set; }
        public List<int> AvailableConnectionsIds { get; set; }

        public LocationModel(ILocationProvider locationProvider, ISessionStorage<GameState> sessionStorage)
        {
            _locationProvider = locationProvider;
            _sessionStorage = sessionStorage;
        }

        private bool IsValidTransition(GameState GameState, int targetLocationId)
        {

            // Definice ID pro speci�ln� lokace
            const int prologId = 1;
            const int shipwreckId = 2;
            const int beachId = 3;
            const int deathId = 8;
            const int endgameId = 9;
            const int indexId = 0;


            switch (targetLocationId)
            {
                case deathId:
                    // Smrt m��e nastat kdykoliv, ale pouze pokud je hr�� mrtv�
                    return GameState.IsPlayerDead;

                case endgameId:
                    // Na konec hry lze p�ej�t pouze pokud hra skon�ila
                    return GameState.HasGameEnded;

                case prologId:
                    // Na prolog lze p�ej�t pouze z indexu
                    return GameState.CurrentLocationId == indexId;

                case shipwreckId:
                    // Na lo� lze p�ej�t z prologu nebo opakovan� z pl�e
                    return GameState.CurrentLocationId == prologId || GameState.CurrentLocationId == beachId;

                case indexId:
                    // Na index lze p�ej�t pouze na za��tku hry
                    return GameState.CurrentLocationId == 0;

                default:
                    // Pro ostatn� lokace plat� standardn� logika
                    return _locationProvider.IsValidConnection(GameState.CurrentLocationId, targetLocationId);
            }
        }


        private void UpdateGameState(GameState GameState, int locationId)
        {
            GameState.CurrentLocationId = locationId;
            _sessionStorage.Save("GameState", GameState);
        }

        private void LoadLocationData(int locationId, bool isRiskyMode)
        {
            CurrentLocation = _locationProvider.GetLocationById(locationId);
            AvailableConnections = _locationProvider.GetConnectionsFromLocation(locationId).ToList();
            AvailableConnectionsIds = new List<int>();

            if (AvailableConnections != null && CurrentLocation != null)
            {
                AvailableConnections.ForEach(singleLocation => {
                    if (CurrentLocation.Id != singleLocation.ToLocationId)
                    {
                        AvailableConnectionsIds.Add(singleLocation.ToLocationId);
                    }
                });
            }

            Tools = new Tools().ToolsList;
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
                        card.ItemCount *= 2;
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
            var GameState = _sessionStorage.LoadOrCreate("GameState");

            GameState.Turns += 1;
            var selectedCard = GetSelectedCard(GameState.CurrentLocationId, cardId);

            if (selectedCard != null)
            {
                if (selectedCard.Item == "enemy")
                {
                    
                    GameState.InFight = true; // Hr�� vstupuje do boje
                    _sessionStorage.Save("GameState", GameState);
                    return RedirectToPage("/Game/Fight", new { cardPackId = (GameState.CurrentLocationId), enemyId = selectedCard.Id });
                    
                }
                else if (selectedCard.Item == "accident") // ne��astn� n�hody co berou �ivoty
                {
                    if(GameState.IsRiskyMode)
                    {
                        GameState.UpdateHealthAndEnergy(selectedCard.ItemCount * 2, 0);
                        _sessionStorage.Save("GameState", GameState);
                        return RedirectToPage("/Game/Location", new { locationId = GameState.CurrentLocationId });
                    }
                    if (!GameState.IsRiskyMode)
                    {
                        GameState.UpdateHealthAndEnergy(selectedCard.ItemCount, 0);
                        _sessionStorage.Save("GameState", GameState);
                        return RedirectToPage("/Game/Location", new { locationId = GameState.CurrentLocationId });
                    }
                }
                else
                {
                    ProcessCard(selectedCard, GameState);
                }

                _sessionStorage.Save("GameState", GameState);
            }

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }

        private IActionResult ProcessCard(Card card, GameState GameState)
        {
            if (GameState.IsRiskyMode)
            {
                card.ItemCount *= 2;
            }
            GameState.AddItem(card.Item, card.ItemCount);

            GameState.CheckGameProgress();

            _sessionStorage.Save("GameState", GameState);

            return Page();
        }

        private Card GetSelectedCard(int currentLocationId, int cardId)
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            var rightCardPack = GameState.CurrentLocationId;
            return new Cards().CardPacks.Find(CardPack => CardPack.Id == currentLocationId).CardsInPack.FirstOrDefault(c => c.Id == cardId);
        }

        public IActionResult OnGet(int locationId)
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            if (!IsValidTransition(GameState, locationId))
            {
                return RedirectToPage("/Game/Cheater");
            }
            if (GameState.InFight)
            {
                return RedirectToPage("/Game/Cheater");
            }
            if (GameState.CurrentLocationId != locationId)
            {
                GameState.IsInventoryOpen = false;
                
            }
            if (GameState.CurrentLocationId != locationId && GameState.CurrentLocationId != 0 && GameState.CurrentLocationId != 1)
            {
                GameState.Turns += 1;
            }

            UpdateGameState(GameState, locationId);

            if (_locationProvider.IsSpecialLocation(locationId))
            {
                return RedirectToSpecialPage(locationId, GameState);
            }

            

            _sessionStorage.Save("GameState", GameState);
            LoadLocationData(locationId, GameState.IsRiskyMode);

            return Page();
        }


        private IActionResult RedirectToSpecialPage(int locationId, GameState GameState)
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
            var GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState.IsRiskyMode = !GameState.IsRiskyMode;
            _sessionStorage.Save("GameState", GameState);

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }

        public IActionResult OnPostHandleBadgeClick(string badgeType)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState.AddTool(badgeType);
            _sessionStorage.Save("GameState", GameState);
            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }

        public IActionResult OnPostHandleChangeLocation(string locationIdInputValue)
        {
            return RedirectToPage(new { locationId = locationIdInputValue });
        }
        
        public IActionResult OnPostRemoveItem(string itemName, int itemCount)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState.IsInventoryOpen = true;

            if (GameState.RemoveItem(itemName, itemCount))
            {
                _sessionStorage.Save("GameState", GameState); 
            }
            _sessionStorage.Save("GameState", GameState);

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }

        public IActionResult OnPostEatItem(string itemName, int itemCount)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState.IsInventoryOpen = true;

            if (itemName == "food" && GameState.Inventory.Items[itemName] >= itemCount)
            {
                GameState.UpdateHealthAndEnergy(1, 0); 

                GameState.RemoveItem(itemName, itemCount);

                _sessionStorage.Save("GameState", GameState);
            }
            _sessionStorage.Save("GameState", GameState);

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }
  

    }
}
