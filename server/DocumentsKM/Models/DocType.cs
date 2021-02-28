using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Тип документа
    public class DocType
    {
        [Key]
        public int Id { get; set; }

        // Шифр
        [Required]
        [MaxLength(4)]
        public string Code { get; set; }

        // Название
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}