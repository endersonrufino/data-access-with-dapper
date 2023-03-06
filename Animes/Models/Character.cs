using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animes.Models
{
    // [Table("Character")]
    public class Character
    {
        // [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int AnimeId { get; set; }
        public Anime Anime { get; set; }
    }
}