using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Раздел общих указаний марки
    public class MarkGeneralDataSection
    {
        [Key]
        public Int32 Id { get; set; }

        // Марка
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Номер
        [Required]
        public Int16 OrderNum { get; set; }

        // Пункты общих указаний
        public virtual IList<MarkGeneralDataPoint> MarkGeneralDataPoints { get; set; } = new List<MarkGeneralDataPoint>();
    }
}
