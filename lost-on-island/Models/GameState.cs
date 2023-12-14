namespace lost_on_island.Models
{
    public class GameState
    {
        public int CurrentLocationId { get; set; } = 0;
        public bool IsPlayerDead { get; set; } = false;
        public bool HasGameEnded { get; set; } = false;

    }

}
