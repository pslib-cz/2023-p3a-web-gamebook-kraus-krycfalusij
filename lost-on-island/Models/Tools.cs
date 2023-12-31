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
                Description = "Snižkuje efekt útoků enemy.",
                Materials = new List<Material>
                {
                    new Material { Title = "Dřevo", Type = "wood", Count = 3 },
                    new Material { Title = "Železo", Type = "iron", Count = 5 }
                }
            },
            new Tool
            {
                Title = "Sekera",
                Type = "Axe",
                 Description = "Zvyšuje množství získaného dřeva.",
                Materials = new List<Material>
                {
                    new Material { Title = "Dřevo", Type = "wood", Count = 3 },
                    new Material { Title = "Kámen", Type = "stone", Count = 5 }
                }
            },
            new Tool
            {
                Title = "Krumpáč",
                Type = "Pickaxe",
                Description = "Dovoluje získat železo.",
                Materials = new List<Material>
                {
                    new Material { Title = "Dřevo", Type = "wood", Count = 3 },
                    new Material { Title = "Kámen", Type = "stone", Count = 5 }
                }
            },
            new Tool
            {
                Title = "Nůžky",
                Type = "Shears",
                 Description = "Dovolují získání vlny.",
                Materials = new List<Material>
                {
                    new Material { Title = "Železo", Type = "iron", Count = 10 }
                }
            },
            new Tool
            {
                Title = "Batoh",
                Type = "Backpack",
                Description = "Zvyšuje maximální množství itemů v inventáři.",
                Materials = new List<Material>
                {
                    new Material { Title = "Vlna", Type = "wool", Count = 10 }
                }
            }
        };
    }

    public class Tool
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public List<Material> Materials { get; set; }
    }

    public class Material
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
    }
}
