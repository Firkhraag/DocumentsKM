using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Длина болта
    public class BoltLength
    {
        [Key]
        public int Id { get; set; }

        // Диаметр
        [Required]
        [ForeignKey("DiameterId")]
        public virtual BoltDiameter Diameter { get; set; }

        // Длина
        [Required]
        public int Length { get; set; }

        // Длина нарезной части болта
        [Required]
        public int ScrewLength { get; set; }

        // Вес
        [Required]
        public float Weight { get; set; }
    }
}
