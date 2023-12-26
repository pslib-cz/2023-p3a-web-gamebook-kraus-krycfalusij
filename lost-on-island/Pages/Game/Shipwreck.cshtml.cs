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

        public List<ShipBuildingPhase> shipBuildingPhases = new List<ShipBuildingPhase>
{
        new ShipBuildingPhase("Op�rn� struktura 1", new Dictionary<string, int>{{"wood", 15}, {"rope", 5}}),
        new ShipBuildingPhase("Op�rn� struktura 2", new Dictionary<string, int>{{"wood", 20}, {"rope", 10}, {"bamboo", 10}}),
        new ShipBuildingPhase("Kabina", new Dictionary<string, int>{{"stone", 20}, {"rope", 5}}),
        new ShipBuildingPhase("Plachta", new Dictionary<string, int>{{"wood", 10}, {"rope", 15}, {"wool", 10}, {"bamboo", 10}}),
        new ShipBuildingPhase("Kormidlo", new Dictionary<string, int>{{"wood", 15}, {"iron", 15}, {"stone", 10}})
};


        public IActionResult OnGet()
        {
            gameState = _sessionStorage.LoadOrCreate("GameState");

            // Ov��en�, zda je u�ivatel na spr�vn�m locationId
            if (gameState.CurrentLocationId != 2 && gameState.CurrentLocationId != 3)
            {
                return RedirectToPage("/Game/Cheater");
            }

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


        public IActionResult OnPostBuildShipPhase(string phaseName)
        {
            gameState = _sessionStorage.LoadOrCreate("GameState");
            var phaseIndex = gameState.CurrentShipBuildingPhaseIndex;
            var phases = shipBuildingPhases;
            var currentPhase = phases.ElementAtOrDefault(phaseIndex);

            if (currentPhase != null && currentPhase.Name == phaseName)
            {
                bool canBuild = true;
                foreach (var material in currentPhase.RequiredMaterials)
                {
                    int playerAmount = gameState.Inventory.Items.GetValueOrDefault(material.Key, 0);
                    if (playerAmount < material.Value)
                    {
                        canBuild = false;
                        gameState.Inventory.Items[material.Key] = 0; // Hr�� vyu�ije v�e co m�
                        currentPhase.RequiredMaterials[material.Key] -= playerAmount; // Sn��me po�adavek na materi�l
                    }
                    else
                    {
                        gameState.Inventory.Items[material.Key] -= material.Value; // Ode�teme materi�l pro postaven�
                    }
                }

                if (canBuild)
                {
                    gameState.CurrentShipBuildingPhaseIndex++; // P�esuneme se na dal�� f�zi
                }

                _sessionStorage.Save("GameState", gameState);
                if (canBuild)
                {
                    // Redirect to confirmation page or refresh with success message
                    return RedirectToPage("/Game/ShipBuiltConfirmation", new { phaseName = phaseName });
                }
                else
                {
                    // Redirect back with error message
                    // (Pro re�lnou aplikaci byste m�li p�idat n�jakou formu u�ivatelsk�ho feedbacku)
                    return RedirectToPage("/Game/Shipwreck");
                }
            }

            // F�ze neodpov�d� aktu�ln�mu indexu nebo neexistuje
            return RedirectToPage("/Game/Shipwreck");
        }

    }
}
