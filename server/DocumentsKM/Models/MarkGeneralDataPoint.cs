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

        // Раздел
        [Required]
        [ForeignKey("SectionId")]
        public virtual MarkGeneralDataSection Section { get; set; }

        // Текст
        [Required]
        public string Text { get; set; }

        // Номер
        [Required]
        public Int16 OrderNum { get; set; }
    }
}
