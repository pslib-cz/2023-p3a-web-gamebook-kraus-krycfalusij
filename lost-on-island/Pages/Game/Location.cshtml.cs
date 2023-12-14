using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace lost_on_island.Pages.Game
{
    public class LocationModel : PageModel
    {
        private readonly ILocationProvider _locationProvider;
        private readonly ISessionStorage<GameState> _sessionStorage;

        public Location CurrentLocation { get; set; }
        public List<Connection> AvailableConnections { get; set; }
        public List<Cards.Card> LocationCards { get; set; }

        public LocationModel(ILocationProvider locationProvider, ISessionStorage<GameState> sessionStorage)
        {
            _locationProvider = locationProvider;
            _sessionStorage = sessionStorage;
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
        }
    }
}
