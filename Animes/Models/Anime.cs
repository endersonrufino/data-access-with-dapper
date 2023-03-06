using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animes.Models
{
    // [Table("Anime")]
    public class Anime
    {
        public Anime()
        {
            Characters = new List<Character>();
        }

        // [Key]
        public int Id { get; set; }
        public string? AnimeName { get; set; }
        public IList<Character> Characters { get; set; }
    }
}