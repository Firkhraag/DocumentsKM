using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Типовое наименование типовой конструкции
    public class StandardConstructionName
    {
        [Key]
        public int Id { get; set; }

        // Наименование
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
