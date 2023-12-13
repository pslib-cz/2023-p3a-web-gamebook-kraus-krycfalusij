using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace lost_on_island.Pages.Game
{
    public class LocationModel : PageModel
    {
        private readonly ILocationProvider _locationProvider;
        private readonly ISessionStorage<GameState> _sessionStorage;
        public Location CurrentLocation { get; set; }
        public List<Connection> AvailableConnections { get; set; }
        public Cards Cards { get; set; }
        public List<Cards.Card> LocationCards { get; set; }

        // START OF CARD
        public int CardId { get; set; }
        public string CardTitle { get; set; }
        public string CardDescription { get; set; }
        public string CardIcon { get; set; }
        public string CardItem { get; set; }
        public int CardItemAdd { get; set; }

        // END OF CARD

        // WORKING WITH CARD ONCLICK
        /*
        private string _message = "Before";

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public void HandleCardClick(int cardId)
        {
            _message = "hello";
       

        }
        */
        // WORKING WITH CARD ONCLICK

        public LocationModel(ILocationProvider locationProvider, ISessionStorage<GameState> sessionStorage)
        {
            _locationProvider = locationProvider;
            _sessionStorage = sessionStorage;

            Cards = new Cards();
        }

        public IActionResult OnGet(int locationId)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            if (gameState.CurrentLocationId == 0)
            {
                gameState.CurrentLocationId = 1; 
                _sessionStorage.Save("GameState", gameState);
            }

            if (gameState.CurrentLocationId != locationId &&
                !_locationProvider.GetConnectionsFromLocation(gameState.CurrentLocationId)
                                 .Any(conn => conn.ToLocationId == locationId))
            {
                return RedirectToPage("/Game/Cheater");
            }

            var location = _locationProvider.GetLocationById(locationId);
            if (location == null)
            {
                return RedirectToPage(new { locationId = gameState.CurrentLocationId });
            }

            gameState.CurrentLocationId = locationId;
            _sessionStorage.Save("GameState", gameState);

            CurrentLocation = location;
            AvailableConnections = _locationProvider.GetConnectionsFromLocation(locationId).ToList();

            
            LocationCards = Cards.CardPacks[locationId - 1].CardsInPack;

            return Page();
        }

    }
}
