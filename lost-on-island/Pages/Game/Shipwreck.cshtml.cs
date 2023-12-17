using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lost_on_island.Pages.Game
{
    public class ShipwreckModel : PageModel
    {
        public GameState GameState { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;
        
        public ShipwreckModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public IActionResult OnGet()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            // Ovìøení, zda je uživatel na správném locationId
            if (GameState.CurrentLocationId != 2 && GameState.CurrentLocationId != 3)
            {
                return RedirectToPage("/Game/Cheater");
            }

            // Aktualizace CurrentLocationId na 2
            GameState.CurrentLocationId = 2;
            _sessionStorage.Save("GameState", GameState);


            return Page();
        }
    }
}
