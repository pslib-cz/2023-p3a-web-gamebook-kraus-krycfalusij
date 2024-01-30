using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using static lost_on_island.Models.Cards;

namespace lost_on_island.Pages.Game
{
    public class LocationModel : PageModel
    {
        private readonly ILogger<LocationModel> _logger;
        private readonly ILocationProvider _locationProvider;
        private readonly ISessionStorage<GameState> _sessionStorage;
        public GameState GameState { get; set; }
        public Location CurrentLocation { get; set; }
        public List<Tool> Tools { get; set; }
        public List<Connection> AvailableConnections { get; set; }
        public List<Cards.Card> LocationCards { get; set; }
        public Card SingleLocationCard { get; set; }
        Random rnd = new Random();
        public int RandomCardIndex { get; set; }
        public List<int> AvailableConnectionsIds { get; set; }

        public LocationModel(ILocationProvider locationProvider, ISessionStorage<GameState> sessionStorage)
        {
            _locationProvider = locationProvider;
            _sessionStorage = sessionStorage;
        }

        private bool IsValidTransition(GameState GameState, int targetLocationId)
        {

            const int prologPortId = 10;
            const int prologOceanId = 11;
            const int prologIslandId = 12;
            const int shipwreckId = 2;
            const int beachId = 3;
            const int deathId = 8;
            const int endgameId = 9;
            const int indexId = 0;


            switch (targetLocationId)
            {
                case deathId:
                    return GameState.IsPlayerDead;

                case endgameId:
                    return GameState.HasGameEnded;

                case prologPortId:
                    return GameState.CurrentLocationId == indexId || GameState.CurrentLocationId == prologOceanId;

                case prologOceanId:
                    return GameState.CurrentLocationId == prologPortId || GameState.CurrentLocationId == prologIslandId;

                case prologIslandId:
                    return GameState.CurrentLocationId == prologOceanId;

                case shipwreckId:
                    return GameState.CurrentLocationId == prologIslandId || GameState.CurrentLocationId == beachId || GameState.CurrentLocationId == prologOceanId || GameState.CurrentLocationId == prologPortId;

                case indexId:
                    return GameState.CurrentLocationId == 0;

                default:
                    return _locationProvider.IsValidConnection(GameState.CurrentLocationId, targetLocationId);
            }
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

        private void UpdateGameState(GameState GameState, int locationId)
        {
            GameState.CurrentLocationId = locationId;
            _sessionStorage.Save("GameState", GameState);
        }

        private void LoadLocationData(int locationId, bool isRiskyMode)
        {
            CurrentLocation = _locationProvider.GetLocationById(locationId);
            AvailableConnections = _locationProvider.GetConnectionsFromLocation(locationId).ToList();
            AvailableConnectionsIds = new List<int>();

            if (AvailableConnections != null && CurrentLocation != null)
            {
                AvailableConnections.ForEach(singleLocation => {
                    if (CurrentLocation.Id != singleLocation.ToLocationId)
                    {
                        AvailableConnectionsIds.Add(singleLocation.ToLocationId);
                    }
                });
            }

            Tools = new Tools().ToolsList;
            LocationCards = new Cards().CardPacks.FirstOrDefault(pack => pack.Id == locationId)?.CardsInPack;

            int totalProbability = LocationCards.Sum(card => card.Probability);
            int randomValue = rnd.Next(1, totalProbability + 1);
            int cumulativeProbability = 0;

            if (isRiskyMode)
            {
                foreach (var card in LocationCards)
                {
                    if (card.Item == "enemy")
                    {
                        card.Probability *= 2;
                    }
                    else
                    {
                        card.ItemCount *= 2;
                    }
                }
            }

            foreach (var card in LocationCards)
            {
                cumulativeProbability += card.Probability;
                if (randomValue <= cumulativeProbability)
                {
                    SingleLocationCard = card;
                    break;
                }
            }

            if (SingleLocationCard.Item == "wood" && GameState.Axe)
            {
                SingleLocationCard.ItemCount *= 2;
            }
            if (SingleLocationCard.Item == "iron" && !GameState.Pickaxe)
            {
                SingleLocationCard.Description = "Jejda, na tohle potøebuješ krumpáè";
                SingleLocationCard.ItemCount = 0;
            }
            if (SingleLocationCard.Item == "wool" && !GameState.Shears)
            {
                SingleLocationCard.Description = "Jejda, na tohle potøebuješ nùžky";
                SingleLocationCard.ItemCount = 0;
            }
        }

        public IActionResult OnPostHandleCardClick(int cardId)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");

            GameState.Turns += 1;
            var selectedCard = GetSelectedCard(GameState.CurrentLocationId, cardId);

            if (selectedCard != null)
            {
                if (selectedCard.Item == "enemy")
                {
                    GameState.InFight = true;
                    GameState.PreviousLocation = _locationProvider.GetLocationById(GameState.CurrentLocationId);

                    _sessionStorage.Save("GameState", GameState);

                    return RedirectToPage("/Game/Fight", new { cardPackId = GameState.CurrentLocationId, enemyId = selectedCard.Id });
                }
                else if (selectedCard.Item == "accident") 
                {
                    GameState.InfoText = "Nehody se stávají.";
                    if (GameState.IsRiskyMode)
                    {
                        GameState.UpdateHealthAndEnergy(selectedCard.ItemCount * 2, 0);
                        _sessionStorage.Save("GameState", GameState);
                        return RedirectToPage("/Game/Location", new { locationId = GameState.CurrentLocationId });
                    }
                    if (!GameState.IsRiskyMode)
                    {
                        GameState.UpdateHealthAndEnergy(selectedCard.ItemCount, 0);
                        _sessionStorage.Save("GameState", GameState);
                        return RedirectToPage("/Game/Location", new { locationId = GameState.CurrentLocationId });
                    }
                }
                else
                {
                    ProcessCard(selectedCard, GameState);
                }

                _sessionStorage.Save("GameState", GameState);
            }

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }

        private IActionResult ProcessCard(Card card, GameState GameState)
        {
            if (GameState.IsRiskyMode)
            {
                card.ItemCount *= 2;
            }
            if ((card.Item == "iron" && !GameState.Pickaxe) || (card.Item == "wool" && !GameState.Shears))
            {
                card.ItemCount = 0;
            }
            else if (card.Item == "wood" && GameState.Axe)
            {
                card.ItemCount *= 2;
            }
            GameState.AddItem(card.Item, card.ItemCount);
            GameState.CheckGameProgress();
            _sessionStorage.Save("GameState", GameState);

            return Page();
        }


        private Card GetSelectedCard(int currentLocationId, int cardId)
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            var rightCardPack = GameState.CurrentLocationId;
            return new Cards().CardPacks.Find(CardPack => CardPack.Id == currentLocationId).CardsInPack.FirstOrDefault(c => c.Id == cardId);
        }

        public IActionResult OnGet(int locationId)
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState.CheckGameProgress();

            if (GameState.ShouldRedirectToDeath)
            {
                return RedirectToPage("/Game/Death");
            }
            else if (GameState.ShouldRedirectToEndGame)
            {
                return RedirectToPage("/Game/EndGame");
            }
            if (!IsValidTransition(GameState, locationId))
            {
                return RedirectToPage("/Game/Cheater");
            }
            if (GameState.InFight)
            {
                return RedirectToPage("/Game/Cheater");
            }

            if (GameState.CurrentLocationId != locationId && GameState.CurrentLocationId != 0 && GameState.CurrentLocationId != 1)
            {
                GameState.UpdateHealthAndEnergy(0, -0.5);
                GameState.Turns += 1;
            }

            UpdateGameState(GameState, locationId);

            if (_locationProvider.IsSpecialLocation(locationId))
            {
                return RedirectToSpecialPage(locationId, GameState);
            }

            _sessionStorage.Save("GameState", GameState);
            LoadLocationData(locationId, GameState.IsRiskyMode);

            return Page();
        }

        
        
        private IActionResult RedirectToSpecialPage(int locationId, GameState GameState)
        {

            switch (locationId)
            {
                
                case 10:
                    return RedirectToPage("/Game/PrologPort");
                case 11:
                    return RedirectToPage("/Game/PrologOcean");
                case 12:
                    return RedirectToPage("/Game/PrologIsland");
                case 2:
                    return RedirectToPage("/Game/Shipwreck");
                case 8: 
                    return RedirectToPage("/Game/Death");
                case 9: 
                    return RedirectToPage("/Game/EndGame");
                case 0:
                    return RedirectToPage("/Index");

                default:
                    return RedirectToPage("/Game/Cheater");
            }
        }

        public IActionResult OnPostToggleRiskyMode()
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");
            GameState.IsRiskyMode = !GameState.IsRiskyMode;
            _sessionStorage.Save("GameState", GameState);

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
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
            if (GameState.Backpack)
                GameState.InventoryCapacity = 40;
            _sessionStorage.Save("GameState", GameState);
            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }


        public IActionResult OnPostHandleChangeLocation(string locationIdInputValue)
        {
            return RedirectToPage(new { locationId = locationIdInputValue });
        }
        
        public IActionResult OnPostRemoveItem(string itemName, int itemCount)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");

            if (GameState.RemoveItem(itemName, itemCount))
            {
                _sessionStorage.Save("GameState", GameState); 
            }
            GameState.InfoText = "Bordel nepotøebuju...";

            _sessionStorage.Save("GameState", GameState);

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }

        public IActionResult OnPostEatItem(string itemName, int itemCount)
        {
            var GameState = _sessionStorage.LoadOrCreate("GameState");

            if (itemName == "food" && GameState.Inventory.Items[itemName] >= itemCount)
            {
                GameState.UpdateHealthAndEnergy(2, 2); 

                GameState.RemoveItem(itemName, itemCount);

                _sessionStorage.Save("GameState", GameState);
            }
            GameState.InfoText = "Jídlo prospívá zdraví.";
            _sessionStorage.Save("GameState", GameState);

            return RedirectToPage(new { locationId = GameState.CurrentLocationId });
        }
  

    }
}
