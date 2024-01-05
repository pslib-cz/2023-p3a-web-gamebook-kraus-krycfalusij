using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lost_on_island.Pages
{
    public class IndexModel : PageModel
    {
        public GameState GameState { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;

        public IndexModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }


        public void OnGet()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState = new GameState();
            GameState.CurrentLocationId = 0;
            _sessionStorage.Save("GameState", GameState);
        }


    }
}