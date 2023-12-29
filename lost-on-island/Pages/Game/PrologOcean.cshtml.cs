using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lost_on_island.Pages.Game
{
    public class PrologOceanModel : PageModel
    {
        private readonly ILocationProvider _locationProvider;
        private readonly ISessionStorage<GameState> _sessionStorage;
        public GameState GameState => _sessionStorage.LoadOrCreate("GameState");
        public Location CurrentLocation { get; set; }


        public PrologOceanModel(ILocationProvider locationProvider, ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
            _locationProvider = locationProvider;
        }

        public IActionResult OnGet()
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            if (gameState.CurrentLocationId != 11 && gameState.CurrentLocationId != 12 && gameState.CurrentLocationId != 10)
            {
                return RedirectToPage("/Game/Cheater");
            }

            gameState.CurrentLocationId = 11;
            _sessionStorage.Save("GameState", gameState);

            return Page();
        }
    }
}
