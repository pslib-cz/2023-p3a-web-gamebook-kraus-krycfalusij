using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lost_on_island.Models;
using lost_on_island.Services;
using System;
using static lost_on_island.Models.Cards;

namespace lost_on_island.Pages.Game
{
    public class FightModel : PageModel
    {
        private readonly ISessionStorage<GameState> _sessionStorage;
        public GameState GameState { get; set; }
        public Card Enemy { get; set; }
        public Question CurrentQuestion { get; set; }

        public FightModel(ISessionStorage<GameState> sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public IActionResult OnGet(int cardPackId, int enemyId)
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            if (!GameState.InFight)
            {
                return RedirectToPage("/Game/Cheater");
            }

            Enemy = GetEnemy(cardPackId, enemyId);
            if (Enemy == null) { return NotFound(); }

            CurrentQuestion = GameState.GetRandomQuestion();
            GameState.CurrentQuestion = CurrentQuestion;
            _sessionStorage.Save("GameState", GameState);

            return Page();
        }

        public IActionResult OnPostAnswer(int enemyId, string userAnswer)
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            CurrentQuestion = GameState.CurrentQuestion;

            if (CurrentQuestion == null)
            {
                return RedirectToPage("/Game/Cheater");
            }

            int damage = CurrentQuestion.EvaluateAnswer(userAnswer);
            if (GameState.Sword)
            {
                if (damage == 1 || damage == 2)
                    damage = 0;
                else
                {
                    damage /= 2;
                }
            }
            Console.WriteLine(damage);

            GameState.UpdateHealth(damage);
            GameState.InFight = false;

            _sessionStorage.Save("GameState", GameState);
            return RedirectToPage("/Game/Location", new { locationId = GameState.PreviousLocation.Id });
        }

        private Card GetEnemy(int cardPackId, int enemyId)
        {
            var cards = new Cards();
            return cards.CardPacks
                        .Find(cardPack => cardPack.Id == cardPackId)
                        ?.CardsInPack
                        .Find(card => card.Id == enemyId);
        }
    }
}
