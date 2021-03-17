using System;
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
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 OrderNum { get; set; }
    }
}
