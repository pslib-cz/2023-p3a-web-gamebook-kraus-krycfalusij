using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace lost_on_island.Pages.Game
{
    public class PrologModel : PageModel
    {
        public GameState GameState { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;

        public PrologModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public IActionResult OnGet()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            // Ovìøení, zda je uživatel na správném locationId
            if (GameState.CurrentLocationId != 0 && GameState.CurrentLocationId != 1)
            {
                return RedirectToPage("/Game/Cheater");
            }

            GameState.CurrentLocationId = 1;
            _sessionStorage.Save("GameState", GameState);


            return Page();
        }
    }
}
