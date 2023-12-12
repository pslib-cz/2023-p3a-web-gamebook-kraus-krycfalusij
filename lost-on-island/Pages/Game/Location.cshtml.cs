using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lost_on_island.Pages.Game
{
    public class LocationModel : PageModel
    {
        private readonly ILocationProvider _locationProvider;
        private readonly ISessionStorage<GameState> _sessionStorage;

        public Location CurrentLocation { get; set; }
        public List<Connection> AvailableConnections { get; set; }

        public LocationModel(ILocationProvider locationProvider, ISessionStorage<GameState> sessionStorage)
        {
            _locationProvider = locationProvider;
            _sessionStorage = sessionStorage;
        }
        
        public void OnGet(int locationId)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            var location = _locationProvider.GetLocationById(locationId);
            if (location == null)
            {
                return;
            }

            gameState.CurrentLocationId = locationId;
            _sessionStorage.Save("GameState", gameState);

            CurrentLocation = location;
            AvailableConnections = _locationProvider.GetConnectionsFromLocation(locationId).ToList();
        }

    }

}
