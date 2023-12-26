using static lost_on_island.Models.Cards;

namespace lost_on_island.Models
{
    public class Inventory
    {
        public Dictionary<string, InventoryItem> Items { get; private set; } = new Dictionary<string, InventoryItem>();

        public Inventory()
        {
            AddItem("wood", 0);
            AddItem("stone", 0);
            AddItem("iron", 0);
            AddItem("rope", 0);
            AddItem("food", 0);
            AddItem("wool", 0);
            AddItem("bamboo", 0);
        }
        
        public void AddItem(string name, int count)
        {

            if (Items.ContainsKey(name))
            {
                Items[name].Count += count;
            }
            else
            {
                Items.Add(name, new InventoryItem { Name = name, Count = count });
            }

        }

        public bool RemoveItem(string name, int count)
        {
            if (Items.ContainsKey(name) && Items[name].Count >= count)
            {
                Items[name].Count -= count;
                return true;
            }
            return false;
        }

        public class InventoryItem
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }
    }

}
