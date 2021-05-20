using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Раздел общих указаний
    public class GeneralDataSection
    {
        [Key]
        public Int16 Id { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Номер
        [Required]
        public Int16 OrderNum { get; set; }

        // Пункты общих указаний
        public virtual IList<GeneralDataPoint> GeneralDataPoints { get; set; } = new List<GeneralDataPoint>();
    }
}
