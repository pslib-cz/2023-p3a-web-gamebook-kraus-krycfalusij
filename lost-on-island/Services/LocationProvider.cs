using lost_on_island.Models;

namespace lost_on_island.Services
{
    public class LocationProvider : ILocationProvider
    {
        private readonly List<Location> _locations;
        private readonly List<Connection> _connections;
        public LocationProvider()
        {

            _locations = new List<Location>();
            _connections = new List<Connection>();

            _locations.Add(new Location { Id = 1, Title = "Loď", ImagePath = "Images/shipwreck.png", Description = "Písečná pláž s výhledem na loď." });
            _locations.Add(new Location { Id = 2, Title = "Les", ImagePath = "Images/forest.png", Description = "Lesík s prasátky." });
            _locations.Add(new Location { Id = 3, Title = "Pláž", ImagePath = "Images/shipwreck.png", Description = "Písečná pláž s výhledem na loď." });
            _locations.Add(new Location { Id = 4, Title = "Hluboký les", ImagePath = "Images/forest.png", Description = "Lesík s prasátky." });



            _connections.Add(new Connection { FromLocationId = 1, ToLocationId = 2, Description = "Les" });
            _connections.Add(new Connection { FromLocationId = 2, ToLocationId = 4, Description = "Hluboký les" });

        }

        public Location GetLocationById(int locationId)
        {
            return _locations.FirstOrDefault(loc => loc.Id == locationId);
        }

        public IEnumerable<Connection> GetConnectionsFromLocation(int locationId)
        {
            return _connections.Where(conn => conn.FromLocationId == locationId);
        }
        public bool IsValidConnection(int currentLocationId, int targetLocationId)
        {
            return _connections.Any(conn => conn.FromLocationId == currentLocationId && conn.ToLocationId == targetLocationId);
        }

    }

}
