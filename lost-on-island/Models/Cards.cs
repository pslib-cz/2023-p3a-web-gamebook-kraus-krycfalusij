namespace lost_on_island.Models
{
    public class Cards
    {
        public List<CardPack> CardPacks { get; set; } = new List<CardPack>
        {
            new CardPack
            {
                Id = 1,
                Type = "BeachPack",
                Cards = new List<Card>
                {
                    new Card { Img = "beach1.jpg", Title = "Sunset on the Beach", Description = "Beautiful sunset view", Item = "Seashell", ItemAdd = 5 },
                    new Card { Img = "beach2.jpg", Title = "Palm Trees", Description = "Scenic palm trees", Item = "Coconut", ItemAdd = 3 },
                }
            },
            new CardPack
            {
                Id = 2,
                Type = "ForestPack",
                Cards = new List<Card>
                {
                    new Card { Img = "forest1.jpg", Title = "Enchanted Forest", Description = "Mysterious and magical", Item = "Magic Mushroom", ItemAdd = 2 },
                    new Card { Img = "forest2.jpg", Title = "Wildlife", Description = "Various forest animals", Item = "Berries", ItemAdd = 4 },
                }
            },
            new CardPack
            {
                Id = 3,
                Type = "CavePack",
                Cards = new List<Card>
                {
                    new Card { Img = "cave1.jpg", Title = "Crystal Cave", Description = "Glowing crystals", Item = "Crystal Shard", ItemAdd = 1 },
                    new Card { Img = "cave2.jpg", Title = "Underground Waterfall", Description = "Hidden beauty", Item = "Underground Flower", ItemAdd = 3 },
                }
            }
        };
        public class CardPack
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public List<Card> Cards { get; set; }
        }

        public class Card
        {
            public string Img { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Item { get; set; }
            public int ItemAdd { get; set; }
        }
    }
}
