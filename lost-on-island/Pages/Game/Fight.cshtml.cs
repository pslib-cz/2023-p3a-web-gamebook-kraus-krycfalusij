using lost_on_island.Models;
using lost_on_island.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static lost_on_island.Models.Cards;

public class FightModel : PageModel
{
    private readonly ISessionStorage<GameState> _sessionStorage;

    // Tøída Cards již není injektovaná jako služba
    public Card Enemy { get; set; }

    public FightModel(ISessionStorage<GameState> sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public void OnGet(int enemyId)
    {
        var gameState = _sessionStorage.LoadOrCreate("GameState");
        var cards = new Cards();  // Vytvoøení instance pøímo zde

        // Najít kartu nepøítele podle enemyId
        Enemy = cards.CardPacks.SelectMany(pack => pack.CardsInPack)
                               .FirstOrDefault(card => card.Id == enemyId && card.Item == "enemy");

        // Mùžete zde pøidat další logiku pro boj nebo kvíz
    }
}
