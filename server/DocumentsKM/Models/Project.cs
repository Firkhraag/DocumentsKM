using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Проект
    public class Project
    {
        [Key]
        public Int32 Id { get; set; }

        // Название
        [Required]
        [MaxLength(550)]
        public string Name { get; set; }

        // Базовая серия
        [Required]
        [MaxLength(20)]
        public string BaseSeries { get; set; }

        // Смещение основной надписи
        [Required]
        public Int16 Bias { get; set; }
    }
}
