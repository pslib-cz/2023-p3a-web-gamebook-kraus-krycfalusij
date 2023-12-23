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

            // Přidání index (hlavní menu) jako speciální lokace s ID 0
            _locations.Add(new Location { Id = 0, Name = "index", Title = "Hlavní Menu", ImagePath = "", Description = "Výchozí stránka hry.", IsSpecial = true });

            // Přidání speciálních lokací
            _locations.Add(new Location { Id = 1, Name = "prolog", Title = "Prolog", ImagePath = "Images/prolog.png", Description = "Začátek dobrodružství.", IsSpecial = true });
            _locations.Add(new Location { Id = 2, Name = "shipwreck", Title = "Loď", ImagePath = "Images/location-shipwreck.png", Description = "Ztroskotaná loď.", IsSpecial = true });

            // Přidání běžných lokací
            _locations.Add(new Location { Id = 3, Name = "beach", Title = "Pláž", ImagePath = "Images/location-beach.png", Description = "Pláž s výhledem na moře." });
            _locations.Add(new Location { Id = 4, Name = "field", Title = "Louka", ImagePath = "Images/location-field.png", Description = "Zelená louka s potulujícími se zvířátky." });
            _locations.Add(new Location { Id = 5, Name = "forest", Title = "Les", ImagePath = "Images/location-forest.png", Description = "Lesík s prasátky." });
            _locations.Add(new Location { Id = 6, Name = "cave", Title = "Jeskyně", ImagePath = "Images/location-cave.png", Description = "Černá díra vedoucí do pekel." });
            _locations.Add(new Location { Id = 7, Name = "deepForest", Title = "Hluboký les", ImagePath = "Images/location-deep-forest.png", Description = "Hluboký les plný překvapení." });

            // Přidání speciálních lokací pro smrt a konec hry
            _locations.Add(new Location { Id = 8, Name = "death", Title = "Smrt", ImagePath = "Images/death.png", Description = "Konec vašeho dobrodružství.", IsSpecial = true });
            _locations.Add(new Location { Id = 9, Name = "endgame", Title = "Konec Hry", ImagePath = "Images/endgame.png", Description = "Gratulujeme! Dostali jste se z ostrova.", IsSpecial = true });

            // Přidání spojení mezi lokacemi
            _connections.Add(new Connection { FromLocationId = 3, ToLocationId = 3, Description = "Pláž" });
            _connections.Add(new Connection { FromLocationId = 4, ToLocationId = 4, Description = "Louka" });
            _connections.Add(new Connection { FromLocationId = 5, ToLocationId = 5, Description = "Les" });
            _connections.Add(new Connection { FromLocationId = 6, ToLocationId = 6, Description = "Jeskyně" });
            _connections.Add(new Connection { FromLocationId = 7, ToLocationId = 7, Description = "Hluboký les" });



            _connections.Add(new Connection { FromLocationId = 3, ToLocationId = 4, Description = "Louka" });
            _connections.Add(new Connection { FromLocationId = 4, ToLocationId = 3, Description = "Pláž" });

            _connections.Add(new Connection { FromLocationId = 2, ToLocationId = 2, Description = "Loď" });
            _connections.Add(new Connection { FromLocationId = 3, ToLocationId = 3, Description = "Pláž" });
            _connections.Add(new Connection { FromLocationId = 4, ToLocationId = 4, Description = "Louka" });
            _connections.Add(new Connection { FromLocationId = 5, ToLocationId = 5, Description = "Les" });
            _connections.Add(new Connection { FromLocationId = 6, ToLocationId = 6, Description = "Jeskyně" });
            _connections.Add(new Connection { FromLocationId = 7, ToLocationId = 7, Description = "Hluboký les" });



            _connections.Add(new Connection { FromLocationId = 6, ToLocationId = 5, Description = "Les" });
            _connections.Add(new Connection { FromLocationId = 5, ToLocationId = 6, Description = "Jeskyně" });


            _connections.Add(new Connection { FromLocationId = 8,ToLocationId = 7, Description = "Hluboký les" });
            _connections.Add(new Connection { FromLocationId = 7, ToLocationId = 7, Description = "Hluboký les" });


            _connections.Add(new Connection { FromLocationId = 4, ToLocationId = 5, Description = "Les" });
            _connections.Add(new Connection { FromLocationId = 5, ToLocationId = 4, Description = "Louka" });




            _connections.Add(new Connection { FromLocationId = 4, ToLocationId = 6, Description = "Jeskyně" });
            _connections.Add(new Connection { FromLocationId = 6, ToLocationId = 4, Description = "Louka" });
            
            _connections.Add(new Connection { FromLocationId = 5, ToLocationId = 7, Description = "Hluboký les" });
            _connections.Add(new Connection { FromLocationId = 7, ToLocationId = 5, Description = "Les" });
            
            _connections.Add(new Connection { FromLocationId = 6, ToLocationId = 7, Description = "Hluboký les" });
            _connections.Add(new Connection { FromLocationId = 7, ToLocationId = 6, Description = "Jeskyně" });

            _connections.Add(new Connection { FromLocationId = 0, ToLocationId = 1, Description = "Prolog" });
            
            _connections.Add(new Connection { FromLocationId = 1, ToLocationId = 2, Description = "Loď" });
            
            _connections.Add(new Connection { FromLocationId = 2, ToLocationId = 3, Description = "Pláž" });
            _connections.Add(new Connection { FromLocationId = 3, ToLocationId = 2, Description = "Loď" });
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

        public bool IsSpecialLocation(int locationId)
        {
            return _locations.Any(loc => loc.Id == locationId && loc.IsSpecial);
        }
    }
}
