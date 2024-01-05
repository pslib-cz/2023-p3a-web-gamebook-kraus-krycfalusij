using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lost_on_island.Pages.Game
{
    public class CheaterModel : PageModel
    {
        public GameState GameState { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;
        public CheaterModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public IActionResult OnGet()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            GameState.CurrentLocationId = 13;

            _sessionStorage.Save("GameState", GameState);

            ResetGameState();
            return Page();
        }

        private void ResetGameState()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState = new GameState();
            GameState.CurrentLocationId = 13;
            _sessionStorage.Save("GameState", GameState);
        }
    }
}
