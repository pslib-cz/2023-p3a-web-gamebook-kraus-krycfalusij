using lost_on_island.Models;


public class Inventory
{
    /*
    public Dictionary<string, InventoryItem> Items { get; private set; } = new Dictionary<string, InventoryItem>() { ["wood"] = new InventoryItem { Name = "...", Count = 0 },
        ["stone"] = new InventoryItem { Name = "...", Count = 0 },
    ["iron"] = new InventoryItem { Name = "...", Count = 0 },
    ["rope"] = new InventoryItem { Name = "...", Count = 0 },
    ["food"] = new InventoryItem { Name = "...", Count = 0 },
        ["wool"] = new InventoryItem { Name = "...", Count = 0 },
        ["bamboo"] = new InventoryItem { Name = "...", Count = 0 }
    };
    */
    public Dictionary<string, int> Items { get; set; } = new Dictionary<string, int>()
    {
        
        ["wood"] = 0,
        ["stone"] = 0,
        ["iron"] = 0,
        ["rope"] = 0,
        ["food"] = 0,
        ["wool"] = 0,
        ["bamboo"] = 0
    };

    public Inventory()
    {
        /*
        AddItem("wood", 0);
        AddItem("stone", 0);
        AddItem("iron", 0);
        AddItem("rope", 0);
        AddItem("food", 0);
        AddItem("wool", 0);
        AddItem("bamboo", 0);
        */
    }
        
    public void AddItem(string name, int count)
    {

        if (Items.ContainsKey(name))
        {
            //Items[name].Count += count;
            Items[name] += count;
        }
        else
        {
            //Items.Add(name, new InventoryItem { Name = name, Count = count });
            Items.Add(name, count);
        }

    }

    public bool RemoveItem(string name, int count)
    {
        if (Items.ContainsKey(name) && Items[name] >= count)
        {
            Items[name] -= count;
            return true;
        }
        return false;
    }

    /*
    public class InventoryItem
    {
        public string Name { get; set; } 
        public int Count { get; set; }
    }
    */
}
