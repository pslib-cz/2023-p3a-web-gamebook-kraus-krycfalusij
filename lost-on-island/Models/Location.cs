namespace lost_on_island.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public bool IsSpecial { get; set; } // Speciální lokace 
    }

}
