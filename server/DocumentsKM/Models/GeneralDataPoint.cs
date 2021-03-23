using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Пункт общих указаний пользователя
    public class GeneralDataPoint
    {
        [Key]
        public Int16 Id { get; set; }

        // Пользователь
        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Раздел
        [Required]
        [ForeignKey("SectionId")]
        public virtual GeneralDataSection Section { get; set; }

        // Текст
        [Required]
        public string Text { get; set; }

        // Номер
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 OrderNum { get; set; }
    }
}
