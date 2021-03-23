using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Грунтовка
    public class Primer
    {
        [Key]
        public Int16 Id { get; set; }

        // Номер группы
        [Required]
        public Int16 GroupNum { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Можно грунтовать
        [Required]
        public bool CanBePrimed { get; set; }

        // Приоритет
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Priority { get; set; }
    }
}
