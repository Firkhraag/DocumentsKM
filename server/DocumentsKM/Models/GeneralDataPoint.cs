using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Пункт общих указаний
    public class GeneralDataPoint
    {
        [Key]
        public Int16 Id { get; set; }

        // Раздел
        [Required]
        [ForeignKey("SectionId")]
        public virtual GeneralDataSection Section { get; set; }

        // Текст
        [Required]
        public string Text { get; set; }

        // Номер
        [Required]
        public Int16 OrderNum { get; set; }
    }
}
