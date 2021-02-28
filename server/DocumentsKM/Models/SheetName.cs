using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Типовое наименование листа основного комплекта
    public class SheetName
    {
        [Key]
        public int Id { get; set; }

        // Наименование
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
