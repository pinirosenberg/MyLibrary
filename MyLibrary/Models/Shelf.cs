using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal? LeftWidth { get; set; }
        public int? GenreId { get; set; }
        public Genre? Genre { get; set; }
        [NotMapped]
        public string genreName { get; set; }

    }
}