using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class SheetName
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // [Key]
        // public Int16 Id { get; set; }

        // [Required]
        // [MaxLength(255)]
        // public string Name { get; set; }
    }
}
