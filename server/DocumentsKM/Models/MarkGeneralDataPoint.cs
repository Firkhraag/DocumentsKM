using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Пункт общих указаний марки
    public class MarkGeneralDataPoint
    {
        [Key]
        public Int32 Id { get; set; }

        // Марка
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

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
