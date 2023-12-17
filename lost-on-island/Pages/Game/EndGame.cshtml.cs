using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lost_on_island.Pages.Game
{
    public class EndGameModel : PageModel
    {
        public GameState GameState { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;

        public EndGameModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public IActionResult OnGet()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            if (GameState.HasGameEnded == false)
            {
                return RedirectToPage("/Game/Cheater");
            }

            GameState.CurrentLocationId = 8;
            _sessionStorage.Save("GameState", GameState);


            return Page();
        }
    }
}
