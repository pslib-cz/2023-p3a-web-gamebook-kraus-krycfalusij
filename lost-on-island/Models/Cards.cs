using System.Collections;

namespace lost_on_island.Models
{
    public class Cards
    {
        public List<CardPack> CardPacks { get; set; } = new List<CardPack>
        {
            new CardPack
            {
                Id = 2,
                Type = "ShipwreckPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 20, Title = "Nic", Img = "", Description = "Nic se tu neděje", Item = "none", ItemCount = 0 },
                }
            },
            new CardPack
            {
                Id = 3,
                Type = "BeachPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 21, Title = "Kokos", Img = "/Images/Icons/coconut.png", Description = "Získal jsi kokos z palmy.", ItemDescription = "jídlo", Item = "food", ItemCount = 1 },
                    new Card { Id = 2, Probability = 21, Title = "Kokos", Img = "/Images/Icons/coconut.png", Description = "Získal jsi dva kokosy z palmy.", ItemDescription = "jídla", Item = "food", ItemCount = 2 },
                    new Card { Id = 3, Probability = 5, Title = "Úpal", Img = "/Images/Icons/sun.png", Description = "Dostal jsi úpal!", ItemDescription = "životy", Item = "accident", ItemCount = -3 },
                    new Card { Id = 4, Probability = 5, Title = "Had", Img = "/Images/Icons/snake.png", Description = "Zaútočil na tebe had!", ItemDescription = "život", Item = "enemy", ItemCount = 0},
                    new Card { Id = 5, Probability = 5, Title = "Pouštní brouk", Img = "/Images/Icons/beatle.png", Description = "Kousl tě divný pouštní brouk", ItemDescription = "životy", Item = "accident", ItemCount = -2 },
                    new Card { Id = 6, Probability = 21, Title = "Bambus", Img = "/Images/Icons/bamboo.png", Description = "Získal jsi bambus.", ItemDescription = "bambus", Item = "bamboo", ItemCount = 1 },
                    new Card { Id = 7, Probability = 21, Title = "Bambus", Img = "/Images/Icons/bamboo.png", Description = "Získal jsi dva bambusy.", ItemDescription = "bambus", Item = "bamboo", ItemCount = 2 }

                }
            },
            new CardPack
            {
                Id = 4,
                Type = "FieldPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 18, Title = "Zajíc", Img = "/Images/Icons/hare.png", Description = "Ulovil jsi zajíce.", ItemDescription = "jídlo", Item = "food", ItemCount = 1 },
                    new Card { Id = 2, Probability = 18, Title = "Srnka", Img = "/Images/Icons/doe.png", Description = "Ulovil jsi srnku.", ItemDescription = "jídlo", Item = "food", ItemCount = 2 },
                    new Card { Id = 3, Probability = 18, Title = "Ovce", Img = "/Images/Icons/sheep.png", Description = "Našel jsi ovci.", ItemDescription = "vlny", Item = "wool", ItemCount = 1 },
                    new Card { Id = 4, Probability = 18, Title = "Ovce", Img = "/Images/Icons/sheep.png", Description = "Našel jsi ovci.", ItemDescription = "vlny", Item = "wool", ItemCount = 2 },
                    new Card { Id = 5, Probability = 8, Title = "Divoké prase", Img = "/Images/Icons/wild-pig.png", Description = "Zaútočilo na tebe divoké prase!", ItemDescription = "život", Item = "enemy", ItemCount = 0 },
                    new Card { Id = 6, Probability = 6, Title = "Brodič řek", Img = "/Images/Icons/river.png", Description = "Zkoušel si přeskočit řeku, ale nějak si to neodhadl.", ItemDescription = "životy", Item = "accident", ItemCount = -2 },
                    new Card { Id = 7, Probability = 6, Title = "Agresivní vosa", Img = "/Images/Icons/wasp.png", Description = "Píchnula tě prolétající vosa.", ItemDescription = "životy", Item = "accident", ItemCount = -2 },
                    new Card { Id = 8, Probability = 6, Title = "Masivní mravenec", Img = "/Images/Icons/ant.png", Description = "Kousnul tě do paty masivní mravenec.", ItemDescription = "životy", Item = "accident", ItemCount = -2 }

                }
            },
            new CardPack
            {
                Id = 5,
                Type = "ForestPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 22, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", ItemDescription = "dřevo", Item = "wood", ItemCount = 1 },
                    new Card { Id = 2, Probability = 22, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", ItemDescription = "dřeva", Item = "wood", ItemCount = 2 },
                    new Card { Id = 3, Probability = 22, Title = "Bobule", Img = "/Images/Icons/berries.png", Description = "Našel jsi bobule.", ItemDescription = "jídlo", Item = "food", ItemCount = 1 },
                    new Card { Id = 4, Probability = 10, Title = "Divoké prase", Img = "/Images/Icons/wild-pig.png", Description = "Zaútočilo na tebe divoké prase!", ItemDescription = "život", Item = "enemy", ItemCount = 0 },
                    new Card { Id = 5, Probability = 12, Title = "Agresivní sršeň", Img = "/Images/Icons/hornet.png", Description = "Píchnula tě lesní sršeň", ItemDescription = "životy", Item = "accident", ItemCount = -3 },
                    new Card { Id = 6, Probability = 12, Title = "Bába pod kořenem", Img = "/Images/Icons/root.png", Description = "Spadl jsi pod kořen a nebylo to příjemné", ItemDescription = "životy", Item = "accident", ItemCount = -2 }


                }
            },
            new CardPack
            {
                Id = 6,
                Type = "CavePack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 16, Title = "Kámen", Img = "/Images/Icons/stone.png", Description = "Získal jsi kámen.", ItemDescription = "kámen", Item = "stone", ItemCount = 1 },
                    new Card { Id = 2, Probability = 16, Title = "Kámen", Img = "/Images/Icons/stone.png", Description = "Získal jsi kameny.", ItemDescription = "kameny", Item = "stone", ItemCount = 2 },
                    new Card { Id = 3, Probability = 10, Title = "Pavouk", Img = "/Images/Icons/spider.png", Description = "Zaútočil na tebe pavouk!", ItemDescription = "život", Item = "enemy", ItemCount = 0 },
                    new Card { Id = 4, Probability = 16, Title = "Železo", Img = "/Images/Icons/iron.png", Description = "Našel jsi železo.", ItemDescription = "železa", Item = "iron", ItemCount = 1 },
                    new Card { Id = 5, Probability = 16, Title = "Železo", Img = "/Images/Icons/iron.png", Description = "Našel jsi železo.", ItemDescription = "železa", Item = "iron", ItemCount = 2 },
                    new Card { Id = 6, Probability = 12, Title = "Nízké stropy", Img = "/Images/Icons/banghead.png", Description = "Nepříjemně si se uhodil o strop jeskyně", ItemDescription = "životy", Item = "accident", ItemCount = -2 },
                    new Card { Id = 7, Probability = 14, Title = "Ulomený kámen", Img = "/Images/Icons/fallingrock.png", Description = "Při kopání se ulomil a spadl na tebe kámen", ItemDescription = "životy", Item = "accident", ItemCount = -3 }

                }
            },
            new CardPack
            {
                Id = 7,
                Type = "DeepForestPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 15, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", ItemDescription = "dřeva", Item = "wood", ItemCount = 3 },
                    new Card { Id = 2, Probability = 15, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", ItemDescription = "dřeva", Item = "wood", ItemCount = 4 },
                    new Card { Id = 3, Probability = 15, Title = "Lano", Img = "/Images/Icons/rope.png", Description = "Získal jsi lano.", ItemDescription = "lano", Item = "rope", ItemCount = 1 },
                    new Card { Id = 4, Probability = 15, Title = "Lano", Img = "/Images/Icons/rope.png", Description = "Získal jsi lano.", ItemDescription = "lana", Item = "rope", ItemCount = 2 },
                    new Card { Id = 5, Probability = 10, Title = "Divoké prase", Img = "/Images/Icons/wild-pig.png", Description = "Zaútočilo na tebe divoké prase!", ItemDescription = "život", Item = "enemy", ItemCount = 0 },
                    new Card { Id = 6, Probability = 10, Title = "Vlk", Img = "/Images/Icons/wolf.png", Description = "Zaútočil na tebe vlk!", ItemDescription = "život", Item = "enemy", ItemCount = 0 },
                    new Card { Id = 7, Probability = 10, Title = "Ohavný komár", Img = "/Images/Icons/mosquito.png", Description = "Pokusil se tě vysát ohavný komár!", ItemDescription = "životy", Item = "accident", ItemCount = -3 },
                    new Card { Id = 8, Probability = 10, Title = "Vlhké liány", Img = "/Images/Icons/lians.png", Description = "Uklouzl si po liáně", ItemDescription = "životy", Item = "accident", ItemCount = -2 }

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
            public string ItemDescription { get; set; }
            public string Item { get; set; }
            public int ItemCount { get; set; } 
            public string ItemType { get; set; }  // druh enemies
        }

    }
}
