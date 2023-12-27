using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static lost_on_island.Models.Cards;

namespace lost_on_island.Pages.Game
{
    public class ShipwreckModel : PageModel
    {
        public GameState gameState { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;


        public Location CurrentLocation { get; set; }
        public List<Connection> AvailableConnections { get; set; }
        public List<Cards.Card> LocationCards { get; set; }
        public Card SingleLocationCard { get; set; }
        Random rnd = new Random();
        public int RandomCardIndex { get; set; }
        public List<int> AvailableConnectionsIds { get; set; }

        private readonly ILocationProvider _locationProvider;
        private readonly ILogger<ShipwreckModel> _logger;
        public ShipwreckModel(ISessionStorage<GameState> sessionStorage, ILocationProvider locationProvider, ILogger<ShipwreckModel> logger)
        {
            _sessionStorage = sessionStorage;
            _locationProvider = locationProvider;
            _logger = logger;
        }


        public IActionResult OnGet()
        {
            gameState = _sessionStorage.LoadOrCreate("GameState");

            // Ov��en�, zda je u�ivatel na spr�vn�m locationId
            if (gameState.CurrentLocationId != 2 && gameState.CurrentLocationId != 3)
            {
                return RedirectToPage("/Game/Cheater");
            }
            Console.WriteLine(gameState.shipBuildingPhases[gameState.CurrentShipBuildingPhaseIndex].RequiredMaterials["wood"]);

            // Aktualizace CurrentLocationId na 2
            gameState.CurrentLocationId = 2;
            gameState.Turns += 1;

            _sessionStorage.Save("GameState", gameState);


            CurrentLocation = _locationProvider.GetLocationById(gameState.CurrentLocationId);

            AvailableConnections = _locationProvider.GetConnectionsFromLocation(2).ToList();
            AvailableConnectionsIds = new List<int>();

            if (AvailableConnections != null && CurrentLocation != null)
            {
                AvailableConnections.ForEach(singleLocation =>
                {
                    if (CurrentLocation.Id != singleLocation.ToLocationId)
                    {
                        AvailableConnectionsIds.Add(singleLocation.ToLocationId);
                    }
                });
            }


            return Page();
        }

        public IActionResult OnPostHandleChangeLocation(string locationIdInputValue)
        {
            if (locationIdInputValue == "3") 
            {
                return RedirectToPage("/Game/Location", new { locationId = 3 });
            }
            return RedirectToPage("/Game/Shipwreck");
        }
                public IActionResult OnPostRemoveItem(string itemName, int itemCount)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");
            if (gameState.RemoveItem(itemName, itemCount))
            {
                _sessionStorage.Save("GameState", gameState); 
            }

            return RedirectToPage(new { locationId = gameState.CurrentLocationId });
        }

        public IActionResult OnPostBuildShipPhase(string phaseName)
        {
            gameState = _sessionStorage.LoadOrCreate("GameState");

            // Zkontrolovat, zda je aktu�ln� f�ze odpov�d� f�zi ur�en� parametrem phaseName
            var currentPhase = gameState.shipBuildingPhases[gameState.CurrentShipBuildingPhaseIndex];
            if (currentPhase != null && currentPhase.Name == phaseName)
            {
                // Sn�it po�adovan� materi�ly pro aktu�ln� f�zi
                gameState.ReduceRequiredMaterials();

                // Zkontrolovat, zda jsou v�echny materi�ly pro tuto f�zi dokon�eny
                bool phaseCompleted = currentPhase.RequiredMaterials.Count == 0 || currentPhase.RequiredMaterials.All(rm => rm.Value <= 0);
                if (phaseCompleted)
                {
                    gameState.CurrentShipBuildingPhaseIndex++; // P�esun na dal�� f�zi, pokud je sou�asn� f�ze kompletn�
                }

                // Ulo�it upraven� GameState zp�t do session storage
                _sessionStorage.Save("GameState", gameState);
                Console.WriteLine(gameState.shipBuildingPhases[0].RequiredMaterials["wood"]);
                return RedirectToPage("/Game/Shipwreck");
            }

            return RedirectToPage("/Game/Shipwreck");
        }
        public IActionResult OnPostEatItem(string itemName, int itemCount)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            if (itemName == "food" && gameState.Inventory.Items[itemName] >= itemCount)
            {
                gameState.UpdateHealthAndEnergy(1, 1);

                gameState.RemoveItem(itemName, itemCount);

                _sessionStorage.Save("GameState", gameState);
            }

            return RedirectToPage(new { locationId = gameState.CurrentLocationId });
        }


    }
}
