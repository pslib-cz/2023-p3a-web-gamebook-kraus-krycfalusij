using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using static lost_on_island.Models.Cards;

namespace lost_on_island.Pages.Game
{
    public class PrologModel : PageModel
    {
        private readonly ILocationProvider _locationProvider;
        private readonly ISessionStorage<GameState> _sessionStorage;
        public GameState GameState => _sessionStorage.LoadOrCreate("GameState");
        public Location CurrentLocation { get; set; }


        public PrologModel(ILocationProvider locationProvider, ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
            _locationProvider = locationProvider;
        }

        public IActionResult OnGet()
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            // Ovìøení, zda je uživatel na správném locationId
            if (gameState.CurrentLocationId != 0 && gameState.CurrentLocationId != 1 && gameState.CurrentLocationId != 8 && gameState.CurrentLocationId != 9)
            {
                return RedirectToPage("/Game/Cheater");
            }

            gameState.CurrentLocationId = 1;
            _sessionStorage.Save("GameState", gameState);

            return Page();
        }
    }
}
