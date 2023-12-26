namespace lost_on_island.Models
{
    public class ShipBuildingPhase
    {
        public string Name { get; set; }
        public Dictionary<string, int> RequiredMaterials { get; set; }

        public ShipBuildingPhase(string name, Dictionary<string, int> requiredMaterials)
        {
            Name = name;
            RequiredMaterials = requiredMaterials;
        }

        // Metoda pro kontrolu, zda hráč má dostatek materiálů pro danou fázi
        public bool HasPlayerEnoughMaterials(Inventory inventory)
        {
            foreach (var requirement in RequiredMaterials)
            {
                if (!inventory.Items.ContainsKey(requirement.Key) || inventory.Items[requirement.Key] < requirement.Value)
                {
                    return false; // Hráč nemá dostatek materiálů
                }
            }
            return true; // Hráč má dostatek materiálů
        }
    }

}
