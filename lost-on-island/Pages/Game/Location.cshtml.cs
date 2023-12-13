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
        public Cards Cards { get; set; }
        public List<Connection> AvailableConnections { get; set; }

        // CARD
        public int CardId { get; set; }
        public string CardTitle { get; set; }
        public string CardDescription { get; set; }
        public string CardIcon { get; set; }
        public string CardItem { get; set; }
        public int CardItemAdd { get; set; }

        public int cardIndex = 0;

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

            CardId = Cards.CardPacks[cardIndex].Cards[cardIndex].Id;
            CardTitle = Cards.CardPacks[cardIndex].Cards[cardIndex].Title;
            CardIcon = Cards.CardPacks[cardIndex].Cards[cardIndex].Img;
            CardDescription = Cards.CardPacks[cardIndex].Cards[cardIndex].Description;
            CardItem = Cards.CardPacks[cardIndex].Cards[cardIndex].Item;
            CardItemAdd = Cards.CardPacks[cardIndex].Cards[cardIndex].ItemAdd;

            return Page();
        }
    }
}
