using lost_on_island.Models;

public class GameState
{
    public int CurrentLocationId { get; set; } = 0;
    public bool IsPlayerDead { get; set; } = false;
    public bool HasGameEnded { get; set; } = false;
    public int Energy { get; set; } = 20;
    public int Health { get; set; } = 20;
    public int Turns { get; set; } = 0;
    public bool InFight { get; set; } = false;
    public bool IsRiskyMode { get; set; } = false;

    
    public bool Sword { get; set; } = false;
    public bool Axe { get; set; } = false;
    public bool Pickaxe { get; set; } = false;
    public bool Shears { get; set; } = false;
    public bool Backpack { get; set; } = false;
    
    public int InventoryCapacity { get; set; } = 20;
    public Inventory Inventory { get; set; } = new Inventory();


    public void AddTool(string badgeType)
    {
        switch (badgeType.ToLower())
        {
            case "sword":
                Sword = true;
                break;
            case "axe":
                Axe = true;
                break;
            case "pickaxe":
                Pickaxe = true;
                break;
            case "shears":
                Shears = true;
                break;
            case "backpack":
                Backpack = true;
                break;
            default:
                break;
        }
    }

    public bool AddItem(string name, int count)
    {
        int currentTotalCount = Inventory.Items.Values.Sum();
        if (currentTotalCount + count > InventoryCapacity)
        {
            return false;
        }

        if (Inventory.Items.ContainsKey(name))
        {
            Inventory.Items[name] += count;
        }
        else
        {
            Inventory.Items.Add(name, count);
        }
        return true;
    }


    public bool RemoveItem(string name, int count)
    {
        if (Inventory.Items.ContainsKey(name) && Inventory.Items[name] >= count)
        {
            Inventory.Items[name] -= count;
            return true;
        }
        return false;
    }

    public void UpdateHealthAndEnergy(int healthChange, int energyChange)
    {
        Health += healthChange;
        Energy += energyChange;
        if (Health <= 0 || Energy <= 0) IsPlayerDead = true;
    }


    public void CheckGameProgress()
    {
        // logika dokončení hry?
    }

}
