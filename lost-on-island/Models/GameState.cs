public class GameState
{
    public int CurrentLocationId { get; set; } = 0;
    public bool IsPlayerDead { get; set; } = false;
    public bool HasGameEnded { get; set; } = false;
    public Dictionary<string, int> Inventory { get; set; } = new Dictionary<string, int>();
    public int Energy { get; set; } = 20;
    public int Health { get; set; } = 20;
    public int Turns { get; set; } = 0;
    public bool InFight { get; set; } = false;


    public void UpdateHealthAndEnergy(int healthChange, int energyChange)
    {
        Health += healthChange;
        Energy += energyChange;
        if (Health <= 0 || Energy <= 0) IsPlayerDead = true;
    }

    public void AddToInventory(string item, int amount)
    {
        if (Inventory.ContainsKey(item))
        {
            Inventory[item] += amount;
        }
        else
        {
            Inventory.Add(item, amount);
        }
    }

    public void CheckGameProgress()
    {
        // Příklad: Kontrola, zda jsou splněny podmínky pro stavbu lodě
        if (Inventory.ContainsKey("wood") && Inventory["wood"] >= 20 &&
            Inventory.ContainsKey("rope") && Inventory["rope"] >= 10)
        {
            // Logika pro stavbu lodě
            // Nastavte HasGameEnded na true, pokud je loď dokončena
            HasGameEnded = true;
        }

        // Další podmínky...
    }

}
