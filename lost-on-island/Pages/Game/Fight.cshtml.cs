using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static lost_on_island.Models.Cards;

public class FightModel : PageModel
{
    private readonly ISessionStorage<GameState> _sessionStorage;

    // T��da Cards ji� nen� injektovan� jako slu�ba
    public Card Enemy { get; set; }

    public FightModel(ISessionStorage<GameState> sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public void OnGet(int enemyId)
    {
        var gameState = _sessionStorage.LoadOrCreate("GameState");
        var cards = new Cards();  // Vytvo�en� instance p��mo zde

        // Naj�t kartu nep��tele podle enemyId
        Enemy = cards.CardPacks.SelectMany(pack => pack.CardsInPack)
                               .FirstOrDefault(card => card.Id == enemyId && card.Item == "enemy");

        // M��ete zde p�idat dal�� logiku pro boj nebo kv�z
    }
}
