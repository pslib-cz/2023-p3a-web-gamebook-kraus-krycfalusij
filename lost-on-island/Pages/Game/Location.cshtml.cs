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
        
        // Zpracov�v� GET po�adavek na str�nku Location
        public IActionResult OnGet(int locationId)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            Console.WriteLine(IsValidTransition(gameState, locationId).ToString());
            // Ov��en� platnosti p�echodu a zpracov�n� speci�ln�ch lokac�
            if (!IsValidTransition(gameState, locationId))
            {
                
                return RedirectToPage("/Game/Cheater");
            }

            UpdateGameState(gameState, locationId);
            LoadLocationData(locationId);

            return Page();
        }

        // Zpracov�v� po�adavek na str�nku s prologem
        private IActionResult HandleProlog(GameState gameState)
        {
            if (gameState.CurrentLocationId == 0)
            {
                UpdateGameState(gameState, 1);
                return RedirectToPage("/Game/Prolog");
            }

            return RedirectToPage("/Game/Cheater");
        }

        // Kontroluje p�echod na stejnou str�nku nebo na jinou ale s platn�m p�echodem
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

        // Na�te data o lokaci
        private void LoadLocationData(int locationId)
        {
            CurrentLocation = _locationProvider.GetLocationById(locationId);
            AvailableConnections = _locationProvider.GetConnectionsFromLocation(locationId).ToList();
            LocationCards = new Cards().CardPacks.FirstOrDefault(pack => pack.Id == locationId)?.CardsInPack;
        }
    }
}
