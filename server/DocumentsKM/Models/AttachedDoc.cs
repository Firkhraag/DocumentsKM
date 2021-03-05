using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Прилагаемый документ марки
    public class AttachedDoc
    {
        [Key]
        public int Id { get; set; }

        // Марка
        [Required]
        public virtual Mark Mark { get; set; }

        // Обозначение
        [Required]
        [MaxLength(40)]
        public string Designation { get; set; }

        // Наименование
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Примечание
        [MaxLength(255)]
        public string Note { get; set; }
    }
}
