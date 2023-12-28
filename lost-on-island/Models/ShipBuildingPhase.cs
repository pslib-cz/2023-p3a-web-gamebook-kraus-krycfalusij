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


        public bool CanBuildPhase()
        {
            return RequiredMaterials.All(rm => rm.Value <= 0);
        }
    }


}
