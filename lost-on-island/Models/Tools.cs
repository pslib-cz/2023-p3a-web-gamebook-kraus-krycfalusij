using System.Collections.Generic;
using static lost_on_island.Models.Cards;

namespace lost_on_island.Models
{
    public class Tools
    {
        public List<Tool> ToolsList { get; set; } = new List<Tool>
        {
            new Tool
            {
                Title = "Meč",
                Type = "Sword",
                Materials = new List<Material>
                {
                    new Material { Title = "dřevo", Type = "wood", Count = 3 },
                    new Material { Title = "železo", Type = "iron", Count = 5 }
                }
            },
            new Tool
            {
                Title = "Sekera",
                Type = "Axe",
                Materials = new List<Material>
                {
                    new Material { Title = "dřevo", Type = "wood", Count = 3 },
                    new Material { Title = "kámen", Type = "stone", Count = 5 }
                }
            },
            new Tool
            {
                Title = "Krumpáč",
                Type = "Pickaxe",
                Materials = new List<Material>
                {
                    new Material { Title = "dřevo", Type = "wood", Count = 3 },
                    new Material { Title = "kámen", Type = "stone", Count = 5 }
                }
            },
            new Tool
            {
                Title = "Nůžky",
                Type = "Shears",
                Materials = new List<Material>
                {
                    new Material { Title = "železo", Type = "iron", Count = 10 }
                }
            },
            new Tool
            {
                Title = "Batoh",
                Type = "Backpack",
                Materials = new List<Material>
                {
                    new Material { Title = "vlna", Type = "wool", Count = 10 }
                }
            }
        };
    }

    public class Tool
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public List<Material> Materials { get; set; }
    }

    public class Material
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
    }
}
