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
                    new Card { Id = 1, Probability = 20, Title = "Nic", Img = "", Description = "Nic se tu neděje", Item = "none", ItemAdd = 0 },
                }
            },
            new CardPack
            {
                Id = 3,
                Type = "BeachPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 17, Title = "Kokos", Img = "/Images/Icons/coconut.png", Description = "Získal jsi kokos z palmy.", Item = "food", ItemAdd = 1 },
                    new Card { Id = 2, Probability = 17, Title = "Kokos", Img = "/Images/Icons/coconut.png", Description = "Získal jsi dva kokosy z palmy.", Item = "food", ItemAdd = 2 },
                    new Card { Id = 3, Probability = 17, Title = "Kokos", Img = "/Images/Icons/coconut.png", Description = "Získal jsi tri kokosy z palmy.", Item = "food", ItemAdd = 3 },
                    new Card { Id = 4, Probability = 17, Title = "Úpal", Img = "/Images/Icons/sun.png", Description = "Dostal jsi úpal!", Item = "accident", ItemAdd = -3 },
                    new Card { Id = 5, Probability = 15, Title = "Had", Img = "Images/Icons/snake.png", Description = "Zaútočil na tebe had!", Item = "enemy", ItemAdd = 0},
                    new Card { Id = 6, Probability = 17, Title = "Pouštní brouk", Img = "/Images/Icons/beatle.png", Description = "Kousl tě divný pouštní brouk", Item = "accident", ItemAdd = -2 },
                }
            },
            new CardPack
            {
                Id = 4,
                Type = "FieldPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 12, Title = "Zajíc", Img = "/Images/Icons/hare.png", Description = "Ulovil jsi zajíce.", Item = "food", ItemAdd = 1 },
                    new Card { Id = 2, Probability = 12, Title = "Srnka", Img = "/Images/Icons/doe.png", Description = "Ulovil jsi srnku.", Item = "food", ItemAdd = 3 },
                    new Card { Id = 3, Probability = 12, Title = "Ovce", Img = "/Images/Icons/sheep.png", Description = "Našel jsi ovci.", Item = "wool", ItemAdd = 2 },
                    new Card { Id = 4, Probability = 12, Title = "Ovce", Img = "/Images/Icons/sheep.png", Description = "Našel jsi ovci.", Item = "wool", ItemAdd = 3 },
                    new Card { Id = 5, Probability = 16, Title = "Divoké prase", Img = "/Images/Icons/wild-pig.png", Description = "Zaútočilo na tebe divoké prase!", Item = "enemy", ItemAdd = 0 },
                    new Card { Id = 6, Probability = 12, Title = "Brodič řek", Img = "/Images/Icons/river.png", Description = "Zkoušel si přeskočit řeku, ale nějak si to neodhadl.", Item = "accident", ItemAdd = -2 },
                    new Card { Id = 7, Probability = 12, Title = "Agresivní vosa", Img = "/Images/Icons/wasp.png", Description = "Píchnula tě prolétající vosa.", Item = "accident", ItemAdd = -2 },
                    new Card { Id = 8, Probability = 12, Title = "Masivní mravenec", Img = "/Images/Icons/ant.png", Description = "Kousnul tě do paty masivní mravenec.", Item = "accident", ItemAdd = -2 },
                }
            },
            new CardPack
            {
                Id = 5,
                Type = "ForestPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 17, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", Item = "wood", ItemAdd = 1 },
                    new Card { Id = 2, Probability = 17, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", Item = "wood", ItemAdd = 2 },
                    new Card { Id = 3, Probability = 17, Title = "Bobule", Img = "/Images/Icons/berries.png", Description = "Našel jsi bobule.", Item = "food", ItemAdd = 1 },
                    new Card { Id = 4, Probability = 15, Title = "Divoké prase", Img = "/Images/Icons/wild-pig.png", Description = "Zaútočilo na tebe divoké prase!", Item = "enemy", ItemAdd = 0 },
                    new Card { Id = 5, Probability = 17, Title = "Agresivní sršeň", Img = "/Images/Icons/hornet.png", Description = "Píchnula tě lesní sršeň", Item = "accident", ItemAdd = -3 },
                    new Card { Id = 6, Probability = 17, Title = "Bába pod kořenem", Img = "/Images/Icons/root.png", Description = "Spadl jsi pod kořen a nebylo to příjemné", Item = "accident", ItemAdd = -2 },

                }
            },
            new CardPack
            {
                Id = 6,
                Type = "CavePack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 14, Title = "Kámen", Img = "/Images/Icons/stone.png", Description = "Získal jsi kámen.", Item = "stone", ItemAdd = 1 },
                    new Card { Id = 2, Probability = 14, Title = "Kámen", Img = "/Images/Icons/stone.png", Description = "Získal jsi kameny.", Item = "wood", ItemAdd = 2 },
                    new Card { Id = 3, Probability = 16, Title = "Pavouk", Img = "/Images/Icons/spider.png", Description = "Zaútočil na tebe pavouk!", Item = "enemy", ItemAdd = 0 },
                    new Card { Id = 4, Probability = 14, Title = "Železo", Img = "/Images/Icons/iron.png", Description = "Našel jsi železo.", Item = "iron", ItemAdd = 2 },
                    new Card { Id = 5, Probability = 14, Title = "Železo", Img = "/Images/Icons/iron.png", Description = "Našel jsi železo.", Item = "iron", ItemAdd = 3 },
                    new Card { Id = 6, Probability = 14, Title = "Nízké stropy", Img = "/Images/Icons/banghead.png", Description = "Nepříjemně si se uhodil o strop jeskyně", Item = "accident", ItemAdd = -2 },
                    new Card { Id = 7, Probability = 14, Title = "Ulomený kámen", Img = "/Images/Icons/fallingrock.png", Description = "Při kopání se ulomil a spadl na tebe kámen", Item = "accident", ItemAdd = -3 },
                }
            },
            new CardPack
            {
                Id = 7,
                Type = "DeepForestPack",
                CardsInPack = new List<Card>
                {
                    new Card { Id = 1, Probability = 13, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", Item = "wood", ItemAdd = 3 },
                    new Card { Id = 2, Probability = 13, Title = "Dřevo", Img = "/Images/Icons/wood.png", Description = "Získal jsi dřevo.", Item = "wood", ItemAdd = 4 },
                    new Card { Id = 3, Probability = 13, Title = "Lano", Img = "/Images/Icons/rope.png", Description = "Získal jsi lano.", Item = "rope", ItemAdd = 1 },
                    new Card { Id = 4, Probability = 13, Title = "Lano", Img = "/Images/Icons/rope.png", Description = "Získal jsi lano.", Item = "rope", ItemAdd = 2 },
                    new Card { Id = 5, Probability = 11, Title = "Divoké prase", Img = "/Images/Icons/wild-pig.png", Description = "Zaútočilo na tebe divoké prase!", Item = "enemy", ItemAdd = 0 },
                    new Card { Id = 6, Probability = 11, Title = "Vlk", Img = "/Images/Icons/wolf.png", Description = "Zaútočil na tebe vlk!", Item = "enemy", ItemAdd = 0 },
                    new Card { Id = 7, Probability = 13, Title = "Ohavný komár", Img = "/Images/Icons/mosquito.png", Description = "Pokusil se tě vysát ohavný komár!", Item = "accident", ItemAdd = -3 },
                    new Card { Id = 8, Probability = 13, Title = "Vlhké liány", Img = "/Images/Icons/lians.png", Description = "Uklouzl si po liáně", Item = "accident", ItemAdd = -2 },
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
            public string ItemType { get; set; }  // druh enemies
        }

    }
}
