using lost_on_island.Models;
using System.Collections.Generic;
using System.Linq;

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

            // Přidání speciálních lokací
            _locations.Add(new Location { Id = 0, Name = "prolog", Title = "Prolog", ImagePath = "Images/prolog.png", Description = "Začátek dobrodružství.", IsSpecial = true });
            _locations.Add(new Location { Id = 1, Name = "shipwreck", Title = "Loď", ImagePath = "Images/shipwreck.png", Description = "Ztroskotaná loď.", IsSpecial = true });
            _locations.Add(new Location { Id = 7, Name = "death", Title = "Smrt", ImagePath = "Images/death.png", Description = "Konec vašeho dobrodružství.", IsSpecial = true });
            _locations.Add(new Location { Id = 8, Name = "endgame", Title = "Konec Hry", ImagePath = "Images/endgame.png", Description = "Gratulujeme! Dostali jste se z ostrova.", IsSpecial = true });
      
            // Přidání běžných lokací
            _locations.Add(new Location { Id = 2, Name = "beach", Title = "Pláž", ImagePath = "Images/beach.png", Description = "Pláž s výhledem na moře." });
            _locations.Add(new Location { Id = 3, Name = "field", Title = "Louka", ImagePath = "Images/field.png", Description = "Zelená louka s potulujícími se zvířátky." });
            _locations.Add(new Location { Id = 4, Name = "forest", Title = "Les", ImagePath = "../Images/forest.png", Description = "Lesík s prasátky." });
            _locations.Add(new Location { Id = 5, Name = "cave", Title = "Jeskyně", ImagePath = "Images/cave.png", Description = "Černá díra vedoucí do pekel." });
            _locations.Add(new Location { Id = 6, Name = "deepForest", Title = "Hluboký les", ImagePath = "Images/deepForest.png", Description = "Hluboký les plný překvapení." });

            // Přidání spojení mezi lokacemi
            _connections.Add(new Connection { FromLocationId = 2, ToLocationId = 3, Description = "Louka" });
            _connections.Add(new Connection { FromLocationId = 3, ToLocationId = 2, Description = "Pláž" });
            _connections.Add(new Connection { FromLocationId = 3, ToLocationId = 4, Description = "Les" });
            _connections.Add(new Connection { FromLocationId = 4, ToLocationId = 3, Description = "Louka" });
            _connections.Add(new Connection { FromLocationId = 3, ToLocationId = 5, Description = "Jeskyně" });
            _connections.Add(new Connection { FromLocationId = 5, ToLocationId = 3, Description = "Louka" });
            _connections.Add(new Connection { FromLocationId = 4, ToLocationId = 6, Description = "Hluboký les" });
            _connections.Add(new Connection { FromLocationId = 6, ToLocationId = 4, Description = "Les" });
            _connections.Add(new Connection { FromLocationId = 5, ToLocationId = 6, Description = "Hluboký les" });
            _connections.Add(new Connection { FromLocationId = 6, ToLocationId = 5, Description = "Jeskyně" });

            _connections.Add(new Connection { FromLocationId = 2, ToLocationId = 1, Description = "Loď" });
            _connections.Add(new Connection { FromLocationId = 1, ToLocationId = 2, Description = "Pláž" });
            _connections.Add(new Connection { FromLocationId = 0, ToLocationId = 1, Description = "Loď" });

        }

        // Vrátí lokaci podle ID
        public Location GetLocationById(int locationId)
        {
            return _locations.FirstOrDefault(loc => loc.Id == locationId);
        }

        // Vrací všechna spojení z lokace s daným ID
        public IEnumerable<Connection> GetConnectionsFromLocation(int locationId)
        {
            return _connections.Where(conn => conn.FromLocationId == locationId);
        }

        // Kontroluje validní cestu mezi lokacemi
        public bool IsValidConnection(int currentLocationId, int targetLocationId)
        {
            return _connections.Any(conn => conn.FromLocationId == currentLocationId && conn.ToLocationId == targetLocationId);
        }

        // Hlídá, zda je lokace speciální
        public bool IsSpecialLocation(int locationId)
        {
            return _locations.Any(loc => loc.Id == locationId && loc.IsSpecial);
        }
    }
}
