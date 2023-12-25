using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lost_on_island.Pages.Game
{
    public class ShipwreckModel : PageModel
    {
        public GameState gameState { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;
        
        public ShipwreckModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public IActionResult OnGet()
        {
            gameState = _sessionStorage.LoadOrCreate("GameState");

            // Ovìøení, zda je uživatel na správném locationId
            if (gameState.CurrentLocationId != 2 && gameState.CurrentLocationId != 3)
            {
                return RedirectToPage("/Game/Cheater");
            }

            // Aktualizace CurrentLocationId na 2
            gameState.CurrentLocationId = 2;
            gameState.Turns += 1;

            _sessionStorage.Save("GameState", gameState);


            return Page();
        }
        public IActionResult OnPostHandleChangeLocation(string locationIdInputValue)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            gameState.Turns += 1;

            _sessionStorage.Save("GameState", gameState);

            return RedirectToPage(new { locationId = locationIdInputValue });
        }
    }
}
