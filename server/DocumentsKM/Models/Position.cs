using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class Position
    {
        // КодДолжности
        [Key]
        public int Id { get; set; }

        // НазваниеДолжности
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        // // КодДолжности
        // [Key]
        // public Int16 Id { get; set; }

        // // НазваниеДолжности
        // [Required]
        // [MaxLength(20)]
        // public string Name { get; set; }
    }
}
