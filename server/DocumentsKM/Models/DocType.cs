using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class DocType
    {
        // Тип_док
        [Key]
        public int Id { get; set; }

        // название_док
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // // Тип_док
        // [Key]
        // public Int16 Id { get; set; }

        // // название_док
        // [Required]
        // [MaxLength(100)]
        // public string Name { get; set; }
    }
}