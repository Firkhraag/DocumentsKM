using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class DocType
    {
        // Тип_док
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(4)]
        public string Code { get; set; }

        // название_док
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}