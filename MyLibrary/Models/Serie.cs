namespace MyLibrary.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public decimal WidthOfAll { get; set; }
        public decimal MaxHeight { get; set; }
        public int ShelfId { get; set; }
        public Shelf Shelf { get; set; }
    }
}