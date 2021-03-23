using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Общие условия эксплуатации
    public class MarkOperatingConditions
    {
        // Марка
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }
        [Key]
        public Int32 MarkId { get; set; }

        // Коэффициент надежности по ответственности
        [Required]
        public float SafetyCoeff { get; set; }

        // Агрессивность среды
        [Required]
        [ForeignKey("EnvAggressivenessId")]
        public virtual EnvAggressiveness EnvAggressiveness { get; set; }

        // Температура
        [Required]
        public Int16 Temperature { get; set; }

        // Зона эксплуатации
        [Required]
        [ForeignKey("OperatingAreaId")]
        public virtual OperatingArea OperatingArea { get; set; }

        // Группа газов
        [Required]
        [ForeignKey("GasGroupId")]
        public virtual GasGroup GasGroup { get; set; }

        // Материал конструкций
        [Required]
        [ForeignKey("ConstructionMaterialId")]
        public virtual ConstructionMaterial ConstructionMaterial { get; set; }

        // Тип лакокрасочного материала
        [Required]
        [ForeignKey("PaintworkTypeId")]
        public virtual PaintworkType PaintworkType { get; set; }

        // Высокопрочные болты
        [Required]
        [ForeignKey("HighTensileBoltsTypeId")]
        public virtual HighTensileBoltsType HighTensileBoltsType { get; set; }
    }
}
