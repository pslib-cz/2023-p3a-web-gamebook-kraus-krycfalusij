namespace lost_on_island.Models
{
    public class Cards
    {
        public List<CardPack> CardPacks { get; set; } = new List<CardPack>
        {
            new CardPack
            {
                Id = 1,
                Type = "ForestPack",
                Cards = new List<Card>
                {
                    new Card { Id = 1, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo!", Item = "wood", ItemAdd = 2 },
                }
            },
        };
        public class CardPack
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public List<Card> Cards { get; set; }
        }

        public class Card
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Img { get; set; }
            public string Description { get; set; }
            public string Item { get; set; }
            public int ItemAdd { get; set; }
        }
    }
}
