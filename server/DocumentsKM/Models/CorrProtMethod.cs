using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Not used now
    // Методы антикоррозионной защиты
    public class CorrProtMethod
    {
        [Key]
        public int Id { get; set; }

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
        public int Status { get; set; }
    }
}
