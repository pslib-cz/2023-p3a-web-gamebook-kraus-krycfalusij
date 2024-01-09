using lost_on_island.Models;


public class Inventory
{
    int capacity;
    public Dictionary<string, int> Items { get; set; }

    public Inventory()
    {
        Items = new Dictionary<string, int>
        {
            ["wood"] = 0,
            ["stone"] = 0,
            ["iron"] = 0,
            ["rope"] = 0,
            ["food"] = 0,
            ["wool"] = 0,
            ["bamboo"] = 0
        };
    }
}
