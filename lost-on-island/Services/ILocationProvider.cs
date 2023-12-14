using lost_on_island.Models;

namespace lost_on_island.Services
{
    public interface ILocationProvider
    {
        Location GetLocationById(int locationId);
        IEnumerable<Connection> GetConnectionsFromLocation(int locationId);
        bool IsSpecialLocation(int locationId);
        bool IsValidConnection(int currentLocationId, int targetLocationId);
    }

}
