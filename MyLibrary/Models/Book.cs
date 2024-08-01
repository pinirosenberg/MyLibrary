using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SerieId { get; set; }
        public Serie? Serie { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        [NotMapped]
        public string? SerieName { get; set; }
        [NotMapped]
        //public int? GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
