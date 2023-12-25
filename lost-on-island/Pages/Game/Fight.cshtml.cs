using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static lost_on_island.Models.Cards;

namespace lost_on_island.Pages.Game
{
    public class FightModel : PageModel
    {
        private readonly ISessionStorage<GameState> _sessionStorage;
        public Card Enemy { get; set; }

        public FightModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public IActionResult OnGet(int cardPackId, int enemyId)
        {
            var gameState = _sessionStorage.LoadOrCreate("GameState");

            // Kontrola, zda je hráè oprávnìnì v boji
            if (!gameState.InFight)
            {
                // Hráè není v boji, pøesmìrování na stránku Cheater
                return RedirectToPage("/Game/Cheater");
            }
            Console.WriteLine("cardPackId" + cardPackId);
            var cards = new Cards();
            Enemy = cards.CardPacks
                .Find(CardPack => CardPack.Id == cardPackId)
                .CardsInPack
                .Find(card => card.Id == enemyId);

            gameState.Turns += 1;

            _sessionStorage.Save("GameState", gameState);
            // Resetujte stav boje na false po dokonèení boje
            // Tuto logiku upravte podle potøeby vaší hry
            //gameState.InFight = false;
            //_sessionStorage.Save("GameState", gameState);

            return Page();
        }
    }
}
