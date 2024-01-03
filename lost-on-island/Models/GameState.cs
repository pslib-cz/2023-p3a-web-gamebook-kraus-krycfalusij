using lost_on_island.Models;

public class GameState
{
    public int CurrentLocationId { get; set; } = 0;
    public bool IsPlayerDead { get; set; } = false;
    public bool HasGameEnded { get; set; } = false;
    public int Energy { get; set; } = 20;
    public int Health { get; set; } = 20;
    public int Turns { get; set; } = -2;
    public bool InFight { get; set; } = false;
    public bool IsRiskyMode { get; set; } = false;
    public string InfoText { get; set; } = "";
    public bool Sword { get; set; } = false;
    public bool Axe { get; set; } = false;
    public bool Pickaxe { get; set; } = false;
    public bool Shears { get; set; } = false;
    public bool Backpack { get; set; } = false;

    public int InventoryCapacity { get; set; } = 20;

    public Location? PreviousLocation { get; set; }

    public Inventory Inventory { get; set; } = new Inventory();
    public int CurrentShipBuildingPhaseIndex { get; set; } = 0;

    public List<ShipBuildingPhase> shipBuildingPhases { get; set; } = new List<ShipBuildingPhase>
    {
            new ShipBuildingPhase("Skelet lodi", new Dictionary<string, int>{ { "wood", 5 }, {"stone", 3} }),
            new ShipBuildingPhase("Pokročilý skelet lodi", new Dictionary<string, int>{{"wood", 20}, {"rope", 10}, {"bamboo", 10}}),
            new ShipBuildingPhase("Kabina", new Dictionary<string, int>{{"stone", 20}, {"rope", 5}}),
            new ShipBuildingPhase("Plachta", new Dictionary<string, int>{{"wood", 10}, {"rope", 15}, {"wool", 10}, {"bamboo", 10}}),
            new ShipBuildingPhase("Kormidlo", new Dictionary<string, int>{{"wood", 15}, {"iron", 15}, {"stone", 10}})
    };


    public bool IsInventoryOpen { get; set; } = false;

    public void OpenInventory()
    {
        IsInventoryOpen = true;
    }

    public void CloseInventory()
    {
        IsInventoryOpen = false;
    }

    public void AddTool(string badgeType)
    {
        switch (badgeType.ToLower())
        {
            case "sword":
                Sword = true;
                break;
            case "axe":
                Axe = true;
                break;
            case "pickaxe":
                Pickaxe = true;
                break;
            case "shears":
                Shears = true;
                break;
            case "backpack":
                Backpack = true;
                break;
            default:
                break;
        }
    }

    public void UpdateInfoText(string message)
    {
        InfoText = message;
    }


    public bool AddItem(string name, int count)
    {
        int currentTotalCount = Inventory.Items.Values.Sum();
        if (currentTotalCount + count > InventoryCapacity)
        {
            InfoText = "Nemůžeš to vzít, máš plný inventář.";
            return false;
        }

        if (Inventory.Items.ContainsKey(name))
        {
            Inventory.Items[name] += count;
        }
        else
        {
            Inventory.Items.Add(name, count);
        }
        UpdateInfoText($"+{count}× {name}");

        return true;
    }

    public bool RemoveItem(string name, int count)
    {
        if (Inventory.Items.ContainsKey(name) && Inventory.Items[name] >= count)
        {
            Inventory.Items[name] -= count;
            UpdateInfoText($"-{count}× {name}");

            return true;
        }
        return false;
    }

    public void UpdateHealthAndEnergy(int healthChange, int energyChange)
    {
        int originalHealth = Health;
        int originalEnergy = Energy;

        Health = Math.Min(20, Health + healthChange);
        Energy = Math.Min(20, Energy + energyChange);

        if (Health != originalHealth)
        {
            UpdateInfoText($"{(Health - originalHealth > 0 ? "+" : "")}{Health - originalHealth} životů");
        }
        if (Energy != originalEnergy)
        {
            UpdateInfoText($"{(Energy - originalEnergy > 0 ? "+" : "")}{Energy - originalEnergy} energie");
        }

        CheckGameProgress();
    }


    public bool ShouldRedirectToEndGame { get; private set; }
    public bool ShouldRedirectToDeath { get; private set; }

    public void CheckGameProgress()
    {
        if (Health <= 0 || Energy <= 0)
        {
            IsPlayerDead = true;
            ShouldRedirectToDeath = true;
        }
        else if (CurrentShipBuildingPhaseIndex >= shipBuildingPhases.Count)
        {
            HasGameEnded = true;
            ShouldRedirectToEndGame = true;
        }
    }


    public void UpdateHealth(int damage)
    {
        Health = Math.Max(0, Health - damage);
        CheckGameProgress();
    }

    public void ReduceRequiredMaterials()
    {
        if (CurrentShipBuildingPhaseIndex >= 0 && CurrentShipBuildingPhaseIndex < shipBuildingPhases.Count)
        {
            var currentPhase = shipBuildingPhases[CurrentShipBuildingPhaseIndex];
            var materialsToRemove = new List<string>();

            foreach (var material in currentPhase.RequiredMaterials.Keys.ToList())
            {
                if (Inventory.Items.ContainsKey(material))
                {
                    int amountToReduce = Math.Min(Inventory.Items[material], currentPhase.RequiredMaterials[material]);
                    Inventory.Items[material] -= amountToReduce;

                    currentPhase.RequiredMaterials[material] -= amountToReduce;
                    
                    Console.WriteLine(currentPhase.RequiredMaterials[material]);
                    
                    if (currentPhase.RequiredMaterials[material] <= 0)
                    {
                        materialsToRemove.Add(material);
                    }
                }
            }

            foreach (var material in materialsToRemove)
            {
                currentPhase.RequiredMaterials.Remove(material);
            }
        }
        CheckGameProgress();

    }


    public Question CurrentQuestion { get; set; }

    public List<Question> Questions { get; set; } = new List<Question>
{
    new Question("Jaký typ uzlu je nejvhodnější pro stavbu přístřešku?", new List<string> {"Námornický uzel", "Klouzavý uzel", "Osmičkový uzel", "Palstek"}, "Námornický uzel"),
    new Question("Jaký materiál je nejlepší pro vytvoření provizorního filtru na vodu?", new List<string> {"Písek", "Uhlí", "Kameny", "Listy"}, "Uhlí"),
    new Question("Který předmět je nejlepší pro získání potravy na opuštěném ostrově?", new List<string> {"Rybářský prut", "Nůž", "Síť", "Luk a šípy"}, "Rybářský prut"),
    new Question("Jaký druh ohně je nejlepší pro vaření?", new List<string> {"Vysoký oheň", "Ohniště s kameny", "Rozdělaný oheň s větvemi", "Uhlíkový oheň"}, "Uhlíkový oheň"),
    new Question("Jakou techniku byste použili k získání vody ze vzduchu v suchém prostředí?", new List<string> {"Kondenzace", "Destilace", "Filtrace", "Absorpce"}, "Kondenzace"),
    new Question("Co je nejlepší udělat, pokud vás v divočině uštkne jedovatý had?", new List<string> {"Vysát jed", "Přeříznout ránu", "Aplikovat led", "Udržet klid a vyhledat lékařskou pomoc"}, "Udržet klid a vyhledat lékařskou pomoc"),
    new Question("Který materiál je nejlepší pro výrobu provizorního nástroje na opuštěném ostrově?", new List<string> {"Dřevo", "Kámen", "Kosti", "Škeble"}, "Kámen"),
    new Question("Jaký typ signálu byste vytvořili na pláži, abyste přilákali pozornost záchranářského letadla?", new List<string> {"Velký kruh", "Dlouhý pruh", "Tři malé kruhy", "Tři rovnoběžné čáry"}, "Tři rovnoběžné čáry"),
    new Question("Jaký druh potravy byste měli vyhledávat jako první na opuštěném ostrově?",
             new List<string> {"Ovoce", "Ryby", "Hmyz", "Listovou zeleninu"}, "Ryby"),
    new Question("Který přírodní materiál je nejlepší pro vytvoření provizorního úkrytu?",
                 new List<string> {"Listy", "Tráva", "Větve", "Bambus"}, "Bambus"),
    new Question("Jaký druh ohně je nejefektivnější pro vysílání signálu nouze?",
                 new List<string> {"Malý a kontrolovaný oheň", "Oheň s mokrým dřevem", "Velký a jasný oheň", "Oheň s přidanými zelenými listy"}, "Oheň s přidanými zelenými listy"),
    new Question("Který přírodní zdroj je nejlepší pro vytvoření nástroje na rybolov?",
                 new List<string> {"Pevný kámen", "Bambus", "Ostří kosti", "Dřevěná větev"}, "Ostří kosti"),
    new Question("Jaký typ přístřešku je nejvhodnější pro tropické klima?",
                 new List<string> {"Hluboká jáma", "Vysoko postavený přístřešek", "Přístřešek s hustými stěnami", "Přístřešek otevřený na všech stranách"}, "Vysoko postavený přístřešek"),
    new Question("Která dovednost je nejvíce kritická pro přežití v arktických podmínkách?",
                 new List<string> {"Umění stavět ohně", "Schopnost lovit", "Znalost první pomoci", "Stavění sněhového přístřešku"}, "Stavění sněhového přístřešku"),
    new Question("Jaký druh rostliny je nejlepší pro získání vlákna na výrobu provazů?",
                 new List<string> {"Borovice", "Kopriva", "Bambus", "Lopuch"}, "Kopriva"),
    new Question("Jaký přírodní materiál je nejlepší pro izolaci těla při nízkých teplotách?",
                 new List<string> {"Mech", "Listí", "Březová kůra", "Tráva"}, "Mech"),
    new Question("Co je nejdůležitější při výběru místa pro tábor na opuštěném ostrově?",
                 new List<string> {"Přístup k vodě", "Otevřené prostranství", "Blízkost potravinových zdrojů", "Výšková poloha"}, "Přístup k vodě"),
    new Question("Který typ signálu byste použili v noci k přilákání pozornosti záchranářů?",
                 new List<string> {"Kouřový signál", "Zrcadlo", "Vysílačka", "Světelný signál"}, "Světelný signál"),
    new Question("Jakou rostlinu byste měli použít pro léčení drobných řezných ran v divočině?",
                 new List<string> {"Pampeliška", "Aloe vera", "Kopřiva", "Lopuch"}, "Aloe vera"),
    new Question("Jaký předmět je nezbytný pro orientaci v neznámém terénu?",
                 new List<string> {"Kompas", "Mapa", "Baterka", "Hodinky"}, "Kompas"),
    new Question("Jaká je nejbezpečnější metoda získávání vody ze zdroje v divočině?",
                 new List<string> {"Přímo pít", "Filtrace", "Vaření", "Sběr rosy"}, "Vaření"),
    new Question("Který typ jídla byste měli prioritně shánět v zimních podmínkách?",
                 new List<string> {"Ovoce", "Zelenina", "Ryby", "Měkkýše"}, "Ryby"),
    new Question("Který přírodní materiál je nejlepší pro výrobu nástrojů na zpracování dřeva?",
                 new List<string> {"Kámen", "Kosti", "Dřevo", "Škeble"}, "Kámen"),

    new Question("Jaký typ úkrytu byste měli stavět v deštném lese?",
                 new List<string> {"Nízký a kompaktní", "Vysoko ve stromech", "Blízko vodního zdroje", "S otevřeným plánem"}, "Vysoko ve stromech"),

    new Question("Jaký přírodní útvar je nejlepší pro orientaci ve volné přírodě?",
                 new List<string> {"Řeka", "Skála", "Vrchol hory", "Údolí"}, "Řeka"),

    new Question("Který typ rostliny je nejlepší pro získání vody v aridních oblastech?",
                 new List<string> {"Kaktus", "Borovice", "Pampeliška", "Lopuch"}, "Kaktus"),
    new Question("Jaké jsou klíčové znaky jedovatých rostlin, kterým byste se měli vyhnout?",
                 new List<string> {"Lesklé listy", "Mléčný latex", "Hladké okraje listů", "Světlé květy"}, "Mléčný latex"),
    new Question("Který nástroj je nejúčinnější pro lov větších zvířat v divočině?",
                 new List<string> {"Pasti", "Luk a šípy", "Oštěp", "Síť"}, "Oštěp")
};


    private Random _random = new Random();

    public Question GetRandomQuestion()
    {
        int index = _random.Next(Questions.Count);
        return Questions[index];
    }
}
