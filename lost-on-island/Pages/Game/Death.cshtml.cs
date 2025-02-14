using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace lost_on_island.Pages.Game
{
    public class DeathModel : PageModel
    {
        public GameState GameState { get; set; }
        public int TotalTurns { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;

        public DeathModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }
        
        public IActionResult OnGet()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            if (GameState.CurrentLocationId != 8 && GameState.IsPlayerDead == false)
            {
                Response.Redirect("/Game/Cheater");
            }

            if (GameState.IsPlayerDead)
                ResetGameState();
            GameState.CurrentLocationId = 8;

            _sessionStorage.Save("GameState", GameState);

            return Page();
        }

        private void ResetGameState()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            TotalTurns = GameState.Turns;
            GameState = new GameState();
            GameState.CurrentLocationId = 8;
            _sessionStorage.Save("GameState", GameState);
        }
    }
}
