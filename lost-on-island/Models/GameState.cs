﻿using lost_on_island.Models;

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

    public bool IsInventoryOpen { get; set; } = false;

    public Inventory Inventory { get; set; } = new Inventory();
    public int CurrentShipBuildingPhaseIndex { get; set; } = 0;

    public List<ShipBuildingPhase> shipBuildingPhases = new List<ShipBuildingPhase>
{
        new ShipBuildingPhase("Skelet lodi", new Dictionary<string, int>{{"wood", 3}}),
        new ShipBuildingPhase("Pokročilý skelet lodi", new Dictionary<string, int>{{"wood", 20}, {"rope", 10}, {"bamboo", 10}}),
        new ShipBuildingPhase("Kabina", new Dictionary<string, int>{{"stone", 20}, {"rope", 5}}),
        new ShipBuildingPhase("Plachta", new Dictionary<string, int>{{"wood", 10}, {"rope", 15}, {"wool", 10}, {"bamboo", 10}}),
        new ShipBuildingPhase("Kormidlo", new Dictionary<string, int>{{"wood", 15}, {"iron", 15}, {"stone", 10}})
};


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
        if (healthChange + Health > 20)
        {
            Health = 20;
        }
        else
        {
            Health += healthChange;
        }
        if (Energy + energyChange > 20)
        {
            Energy = 20;
        }
        else
        {
            Energy += energyChange;
        }
        if (Health <= 0 || Energy <= 0) IsPlayerDead = true;
    }


    public void CheckGameProgress()
    {
        // logika dokončení hry?
    }

    public void ReduceRequiredMaterials()
    {
        if (CurrentShipBuildingPhaseIndex >= 0 && CurrentShipBuildingPhaseIndex < shipBuildingPhases.Count)
        {
            var currentPhase = shipBuildingPhases[CurrentShipBuildingPhaseIndex];
            var materialsToRemove = new List<string>();

            foreach (var material in currentPhase.RequiredMaterials.Keys.ToList())
            {
                if (Inventory.Items.ContainsKey(material))
                {
                    int amountToReduce = Math.Min(Inventory.Items[material], currentPhase.RequiredMaterials[material]);
                    Inventory.Items[material] -= amountToReduce;
                    currentPhase.RequiredMaterials[material] -= amountToReduce;
                    Console.WriteLine(currentPhase.RequiredMaterials[material]);
                    if (currentPhase.RequiredMaterials[material] <= 0)
                    {
                        materialsToRemove.Add(material);
                    }
                }
            }

            // Odstranění materiálů s nulovým nebo záporným množstvím
            foreach (var material in materialsToRemove)
            {
                currentPhase.RequiredMaterials.Remove(material);
            }
        }
    }

}
