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

        // Zpracovává požadavek na stránku s prologem
        private IActionResult HandleProlog(GameState gameState)
        {
            if (gameState.CurrentLocationId == 0)
            {
                UpdateGameState(gameState, 1);
                return RedirectToPage("/Game/Prolog");
            }

            return RedirectToPage("/Game/Cheater");
        }

        // Kontroluje pøechod na stejnou stránku nebo na jinou ale s platným pøechodem
        private bool IsValidTransition(GameState gameState, int locationId)
        {
            return gameState.CurrentLocationId == locationId ||
                   _locationProvider.IsValidConnection(gameState.CurrentLocationId, locationId);
        }

        // Aktualizuje stav hry
        private void UpdateGameState(GameState gameState, int locationId)
        {
            gameState.CurrentLocationId = locationId;
            _sessionStorage.Save("GameState", gameState);
        }

        // Naète data o lokaci
        private void LoadLocationData(int locationId)
        {
            CurrentLocation = _locationProvider.GetLocationById(locationId);
            AvailableConnections = _locationProvider.GetConnectionsFromLocation(locationId).ToList();
            LocationCards = new Cards().CardPacks.FirstOrDefault(pack => pack.Id == locationId)?.CardsInPack;

            // Výbìr náhodné karty na bázi probility vlastnosti
            int totalProbability = LocationCards.Sum(card => card.Probability);
            int randomValue = rnd.Next(1, totalProbability + 1);
            int cumulativeProbability = 0;
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

        // KARTY
        public IActionResult OnPostHandleCardClick(int cardId)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            // Naètení vybrané karty
            var selectedCard = GetSelectedCard(cardId);

            if (selectedCard != null)
            {
                ProcessCard(selectedCard, gameState);
                _sessionStorage.Save("GameState", gameState);
            }

            return RedirectToPage(new { locationId = gameState.CurrentLocationId });
        }

        private Card GetSelectedCard(int cardId)
        {
            foreach (var pack in new Cards().CardPacks)
            {
                var foundCard = pack.CardsInPack.FirstOrDefault(c => c.Id == cardId);
                if (foundCard != null)
                {
                    return foundCard;
                }
            }
            return null;
        }

        private void ProcessCard(Card card, GameState gameState)
        {
            // Pøidání položek do inventáøe
            gameState.AddToInventory(card.Item, card.ItemAdd);

            // Aktualizace zdraví nebo energie
            if (card.Item == "food")
            {
                gameState.UpdateHealthAndEnergy(5, 0); // Pøidá zdraví
            }
            else if (card.Item == "enemy")
            {
                gameState.UpdateHealthAndEnergy(-10, 0); // Odeète zdraví
            }

            // Kontrola postupu ve høe
            gameState.CheckGameProgress();
        }

        // Zpracovává GET požadavek na stránku Location
        public IActionResult OnGet(int locationId)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            Console.WriteLine(IsValidTransition(gameState, locationId).ToString());
            // Ovìøení platnosti pøechodu a zpracování speciálních lokací
            if (!IsValidTransition(gameState, locationId))
            {
                return RedirectToPage("/Game/Cheater");
            }

            UpdateGameState(gameState, locationId);
            LoadLocationData(locationId);

            return Page();
        }
    }
}
