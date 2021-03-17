using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Методы антикоррозионной защиты
    public class CorrProtMethod
    {
        [Key]
        public Int16 Id { get; set; }

        // Агрессивность среды
        [Required]
        [ForeignKey("EnvAggressivenessId")]
        public virtual EnvAggressiveness EnvAggressiveness { get; set; }

        // Материал конструкций
        [Required]
        [ForeignKey("ConstructionMaterialId")]
        public virtual ConstructionMaterial ConstructionMaterial { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Статус
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Status { get; set; }
    }
}
