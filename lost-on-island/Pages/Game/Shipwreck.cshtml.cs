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


            if (GameState.CurrentLocationId != 2 && GameState.CurrentLocationId != 3 && GameState.CurrentLocationId != 8)
            {
                return RedirectToPage("/Game/Cheater");
            }

            if (GameState.CurrentLocationId != 2)
            {
                GameState.IsInventoryOpen = false;
            }

            GameState.CurrentLocationId = 2;

            if (GameState.CurrentLocationId != 2 && GameState.CurrentLocationId != 0 && GameState.CurrentLocationId != 1)
            {
                GameState.UpdateHealthAndEnergy(0, -0.5);
                GameState.Turns += 1;
            }
            GameState.InfoText = "Zde opravuješ svou loï.";
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

        public IActionResult OnPostHideTutorial()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState.ShowTutorial = false;
            _sessionStorage.Save("GameState", GameState);
            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }
        public IActionResult OnPostShowTutorial()
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState.ShowTutorial = true;
            _sessionStorage.Save("GameState", GameState);
            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }
        public IActionResult OnPostHandleChangeLocation(string locationIdInputValue)
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            if (locationIdInputValue == "3") 
            {
                GameState.InfoText = "";
                _sessionStorage.Save("GameState", GameState);
                return RedirectToPage("/Game/Location", new { locationId = 3 });
            }
            return RedirectToPage("/Game/Shipwreck");
        }

        public IActionResult OnPostBuildShipPhase(string phaseName)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");
            var currentPhase = GameState.shipBuildingPhases[GameState.CurrentShipBuildingPhaseIndex];

            if (currentPhase != null && currentPhase.Name == phaseName)
            {
                GameState.ReduceRequiredMaterials();
                GameState.InfoText = "Opravuješ loï!";

                bool phaseCompleted = currentPhase.RequiredMaterials.Count == 0 || currentPhase.RequiredMaterials.All(rm => rm.Value <= 0);
                if (phaseCompleted)
                {
                    GameState.InfoText = "Dokonèil jsi aktuální fázi lodi!";
                    GameState.CurrentShipBuildingPhaseIndex++;
                }

                GameState.CheckGameProgress();
                _sessionStorage.Save("GameState", GameState);

                if (GameState.IsPlayerDead)
                {
                    return RedirectToPage("/Game/Death");
                }
                else if (GameState.HasGameEnded)
                {
                    return RedirectToPage("/Game/EndGame");
                }

                return RedirectToPage("/Game/Shipwreck");
            }

            return RedirectToPage("/Game/Shipwreck");
        }


        public IActionResult OnPostHandleBadgeClick(string badgeType)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");
            Tools = new Tools().ToolsList;
            var tool = Tools.FirstOrDefault(t => t.Type == badgeType);
            foreach (var material in tool.Materials)
            {
                GameState.RemoveItem(material.Type, material.Count);
            }
            GameState.AddTool(badgeType);
            _sessionStorage.Save("GameState", GameState);
            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }


        public IActionResult OnPostRemoveItem(string itemName, int itemCount)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");

            if (GameState.RemoveItem(itemName, itemCount))
            {
                _sessionStorage.Save("GameState", GameState);
            }
            _sessionStorage.Save("GameState", GameState);

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }

        public IActionResult OnPostEatItem(string itemName, int itemCount)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");

            if (itemName == "food" && GameState.Inventory.Items[itemName] >= itemCount)
            {
                GameState.UpdateHealthAndEnergy(1, 0);

                GameState.RemoveItem(itemName, itemCount);

                _sessionStorage.Save("GameState", GameState);
            }
            _sessionStorage.Save("GameState", GameState);

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }


    }
}
