using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static lost_on_island.Models.Cards;

namespace lost_on_island.Pages.Game
{
    public class ShipwreckModel : PageModel
    {
        public GameState GameState { get; set; }
        private readonly ISessionStorage<GameState> _sessionStorage;


        public Location CurrentLocation { get; set; }
        public List<Connection> AvailableConnections { get; set; }
        public List<Cards.Card> LocationCards { get; set; }
        public List<Tool> Tools { get; set; }
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
            Tools = new Tools().ToolsList;
            GameState = _sessionStorage.LoadOrCreate("GameState");

            // Ovìøení, zda je uživatel na správném locationId
            if (GameState.CurrentLocationId != 2 && GameState.CurrentLocationId != 3)
            {
                return RedirectToPage("/Game/Cheater");
            }

            if (GameState.CurrentLocationId != 2)
            {
                GameState.IsInventoryOpen = false;
            }

            // Aktualizace CurrentLocationId na 2
            GameState.CurrentLocationId = 2;
            GameState.Turns += 1;

            _sessionStorage.Save("GameState", GameState);

            CurrentLocation = _locationProvider.GetLocationById(GameState.CurrentLocationId);

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
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            // Zkontrolovat, zda je aktuální fáze odpovídá fázi urèené parametrem phaseName
            var currentPhase = gameState.shipBuildingPhases[gameState.CurrentShipBuildingPhaseIndex];
            if (currentPhase != null && currentPhase.Name == phaseName)
            {
                // Snížit požadované materiály pro aktuální fázi
                gameState.ReduceRequiredMaterials();

                // Zkontrolovat, zda jsou všechny materiály pro tuto fázi dokonèeny
                bool phaseCompleted = currentPhase.RequiredMaterials.Count == 0 || currentPhase.RequiredMaterials.All(rm => rm.Value <= 0);
                if (phaseCompleted)
                {
                    gameState.CurrentShipBuildingPhaseIndex++; // Pøesun na další fázi, pokud je souèasná fáze kompletní
                }

                // Uložit upravený GameState zpìt do session storage
                _sessionStorage.Save("GameState", gameState);
                return RedirectToPage("/Game/Shipwreck");
            }

            return RedirectToPage("/Game/Shipwreck");
        }


        
        public IActionResult OnPostRemoveItem(string itemName, int itemCount)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");
            gameState.IsInventoryOpen = true;

            if (gameState.RemoveItem(itemName, itemCount))
            {
                _sessionStorage.Save("GameState", gameState);
            }
            _sessionStorage.Save("GameState", gameState);

            return RedirectToPage(new { locationId = gameState.CurrentLocationId });
        }

        public IActionResult OnPostEatItem(string itemName, int itemCount)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");
            gameState.IsInventoryOpen = true;

            if (itemName == "food" && gameState.Inventory.Items[itemName] >= itemCount)
            {
                gameState.UpdateHealthAndEnergy(1, 0);

                gameState.RemoveItem(itemName, itemCount);

                _sessionStorage.Save("GameState", gameState);
            }
            _sessionStorage.Save("GameState", gameState);

            return RedirectToPage(new { locationId = gameState.CurrentLocationId });
        }


    }
}
