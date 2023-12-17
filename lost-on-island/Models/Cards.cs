using System.Collections;

namespace lost_on_island.Models
{
    public class Cards
    {
        public List<CardPack> CardPacks { get; set; } = new List<CardPack>
        {
            new CardPack
            {
                Id = 1,
                Type = "ShipwreckPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 20, Title = "Nic", Img = "", Description = "Nic se tu neděje", Item = "none", ItemAdd = 0 },
                }
            },
            new CardPack
            {
                Id = 2,
                Type = "BeachPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 20, Title = "Nic", Img = "", Description = "Nic se tu neděje", Item = "none", ItemAdd = 0 },
                }
            },
            new CardPack
            {
                Id = 3,
                Type = "FieldPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 20, Title = "Zajíc", Img = "/Images/Icons/hare.png", Description = "Ulovil jsi zajíce.", Item = "food", ItemAdd = 1 },
                    new Card { Id = 2, Probability = 20, Title = "Srnka", Img = "/Images/Icons/doe.png", Description = "Ulovil jsi srnku.", Item = "food", ItemAdd = 3 },
                    new Card { Id = 3, Probability = 20, Title = "Ovce", Img = "/Images/Icons/sheep.png", Description = "Našel jsi ovci.", Item = "wool", ItemAdd = 2 },
                    new Card { Id = 4, Probability = 20, Title = "Ovce", Img = "/Images/Icons/sheep.png", Description = "Našel jsi ovci", Item = "wool", ItemAdd = 3 },
                    new Card { Id = 5, Probability = 20, Title = "Divoké prase", Img = "/Images/Icons/wild-pig.png", Description = "Zaútočilo na tebe divoké prase", Item = "enemy", ItemAdd = 0 },
                }
            },
            new CardPack
            {
                Id = 4,
                Type = "ForestPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 20, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", Item = "wood", ItemAdd = 2 },
                    new Card { Id = 2, Probability = 20, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", Item = "wood", ItemAdd = 3 },
                    new Card { Id = 3, Probability = 20, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", Item = "wood", ItemAdd = 4 },
                    new Card { Id = 4, Probability = 20, Title = "Bobule", Img = "/Images/Icons/berries.png", Description = "Našel jsi bobule.", Item = "food", ItemAdd = 1 },
                    new Card { Id = 5, Probability = 20, Title = "Divoké prase", Img = "/Images/Icons/wild-pig.png", Description = "Zaútočilo na tebe divoké prase.", Item = "enemy", ItemAdd = 0 },
                }
            },
            new CardPack
            {
                Id = 5,
                Type = "CavePack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 20, Title = "Kámen", Img = "/Images/Icons/stone.png", Description = "Získal jsi kámen.", Item = "stone", ItemAdd = 1 },
                    new Card { Id = 2, Probability = 20, Title = "Kámen", Img = "/Images/Icons/stone.png", Description = "Získal jsi kameny.", Item = "wood", ItemAdd = 2 },
                    new Card { Id = 3, Probability = 20, Title = "Pavouk", Img = "/Images/Icons/spider.png", Description = "Kousl tě pavouk.", Item = "damage", ItemAdd = -3 },
                    new Card { Id = 4, Probability = 20, Title = "Železo", Img = "/Images/Icons/iron.png", Description = "Našel jsi železo.", Item = "iron", ItemAdd = 2 },
                    new Card { Id = 5, Probability = 20, Title = "Železo", Img = "/Images/Icons/iron.png", Description = "Našel jsi železo.", Item = "iron", ItemAdd = 3 },
                }
            },
            new CardPack
            {
                Id = 6,
                Type = "DeepForestPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 20, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", Item = "wood", ItemAdd = 3 },
                    new Card { Id = 2, Probability = 20, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", Item = "wood", ItemAdd = 4 },
                    new Card { Id = 3, Probability = 20, Title = "Lano", Img = "/Images/Icons/rope.png", Description = "Získal jsi lano.", Item = "rope", ItemAdd = 1 },
                    new Card { Id = 4, Probability = 20, Title = "Lano", Img = "/Images/Icons/rope.png", Description = "Získal jsi lano.", Item = "rope", ItemAdd = 2 },
                    new Card { Id = 5, Probability = 20, Title = "Divoké prase", Img = "/Images/Icons/wolf.png", Description = "Zaútočil na tebe vlk.", Item = "enemy", ItemAdd = 0 },
                }
            },
        };
        public class CardPack
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public List<Card> CardsInPack { get; set; }
        }

        public class Card
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int Probability { get; set; }
            public string Img { get; set; }
            public string Description { get; set; }
            public string Item { get; set; }
            public int ItemAdd { get; set; }
        }
    }
}
