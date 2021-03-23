using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Длина болта
    public class BoltLength
    {
        [Key]
        public Int16 Id { get; set; }

        // Диаметр
        [Required]
        [ForeignKey("DiameterId")]
        public virtual BoltDiameter Diameter { get; set; }

        // Длина
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Length { get; set; }

        // Длина нарезной части болта
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 ScrewLength { get; set; }

        // Вес
        [Required]
        public float Weight { get; set; }
    }
}
