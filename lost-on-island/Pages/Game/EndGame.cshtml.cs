using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace lost_on_island.Pages.Game
{
    public class EndGameModel : PageModel
    {
        public GameState GameState { get; set; }
        public int TotalTurns { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;

        public EndGameModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public IActionResult OnGet()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            if (GameState.CurrentLocationId != 9 && GameState.HasGameEnded == false)
            {
                Response.Redirect("/Game/Cheater");
            }

            GameState.CurrentLocationId = 9;
            GameState.Turns += 1;
            
            _sessionStorage.Save("GameState", GameState);

            ResetGameState();
            return Page();
        }

        private void ResetGameState()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            TotalTurns = GameState.Turns;
            GameState = new GameState();
            GameState.CurrentLocationId = 9;
            _sessionStorage.Save("GameState", GameState);
        }
    }
}
