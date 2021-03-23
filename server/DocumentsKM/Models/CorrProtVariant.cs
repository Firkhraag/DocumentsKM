using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Not used now
    // Вариант антикоррозионной защиты
    public class CorrProtVariant
    {
        [Key]
        public Int16 Id { get; set; }

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
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? PaintworkGroup { get; set; }

        // Стойкость
        [ForeignKey("PaintworkFastnessId")]
        public virtual PaintworkFastness PaintworkFastness { get; set; }

        // Число слоев
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? PaintworkNumOfLayers { get; set; }

        // Толщина покраски
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? PaintworkPrimerThickness { get; set; }

        // Количество слоев грунтовки
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? PrimerNumOfLayers { get; set; }

        // Степень очистки
        [ForeignKey("CleaningDegreeId")]
        public virtual CorrProtCleaningDegree CleaningDegree { get; set; }

        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Status { get; set; }
    }
}
