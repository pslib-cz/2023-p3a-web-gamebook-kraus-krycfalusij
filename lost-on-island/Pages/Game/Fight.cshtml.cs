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

            // Ov��en�, zda jsme v bojov�m re�imu
            if (!GameState.InFight)
            {
                return RedirectToPage("/Game/Cheater");
            }

            Enemy = GetEnemy(cardPackId, enemyId);
            if (Enemy == null) { return NotFound(); }

            // Z�sk�n� n�hodn� ot�zky
            CurrentQuestion = GameState.GetRandomQuestion();
            GameState.CurrentQuestion = CurrentQuestion; // Ulo�en� aktu�ln� ot�zky do GameState
            _sessionStorage.Save("GameState", GameState);

            return Page();
        }


        public IActionResult OnPostAnswer(int enemyId, string userAnswer)
        {
            GameState = _sessionStorage.LoadOrCreate("GameState");

            // Na�ten� ot�zky z GameState
            CurrentQuestion = GameState.CurrentQuestion;

            if (CurrentQuestion == null)
            {
                // Zpracov�n� situace, kdy ot�zka nen� dostupn�
                return RedirectToPage("/Game/Cheater");
            }

            int damage = CurrentQuestion.EvaluateAnswer(userAnswer);
            GameState.UpdateHealth(damage);
            GameState.InFight = false;

            // Ulo�en� stavu hry a p�esm�rov�n� na p�edchoz� lokaci
            _sessionStorage.Save("GameState", GameState);
            return RedirectToPage("/Game/Location", new { locationId = GameState.PreviousLocationId });
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
