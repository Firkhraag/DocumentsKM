using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Not used now
    // Вариант антикоррозионной защиты
    public class CorrProtVariant
    {
        [Key]
        public int Id { get; set; }

        // Зона эксплуатации
        [Required]
        [ForeignKey("OperatingAreaId")]
        public virtual OperatingArea OperatingArea { get; set; }

        // Группа газов
        [Required]
        [ForeignKey("GasGroupId")]
        public virtual GasGroup GasGroup { get; set; }

        // Агрессивность среды
        [Required]
        [ForeignKey("EnvAggressivenessId")]
        public virtual EnvAggressiveness EnvAggressiveness { get; set; }

        // Материал конструкций
        [Required]
        [ForeignKey("ConstructionMaterialId")]
        public virtual ConstructionMaterial ConstructionMaterial { get; set; }

        // Тип лакокрасочного материала
        [ForeignKey("PaintworkTypeId")]
        public virtual PaintworkType PaintworkType { get; set; }

        // Группа покраски
        public int? PaintworkGroup { get; set; }

        // Стойкость
        [ForeignKey("PaintworkFastnessId")]
        public virtual PaintworkFastness PaintworkFastness { get; set; }

        // Число слоев
        public int? PaintworkNumOfLayers { get; set; }

        // Толщина покраски
        public int? PaintworkPrimerThickness { get; set; }

        // Количество слоев грунтовки
        public int? PrimerNumOfLayers { get; set; }

        // Степень очистки
        [ForeignKey("CleaningDegreeId")]
        public virtual CorrProtCleaningDegree CleaningDegree { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
