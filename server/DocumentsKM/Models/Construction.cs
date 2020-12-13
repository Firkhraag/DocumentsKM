using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Construction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("SpecificationId")]
        public virtual Specification Specification { get; set; }

        [Required]
        [ForeignKey("ConstructionTypeId")]
        public virtual ConstructionType ConstructionType { get; set; }

        [Required]
        [ForeignKey("SpecificationId")]
        public virtual ConstructionSubtype ConstructionSubtype { get; set; }

        // Код_отд
        [Required]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        // Гл_спец
        [ForeignKey("ChiefSpecialistId")]
        public virtual Employee ChiefSpecialist { get; set; }

        // Рук_гр 
        [ForeignKey("GroupLeaderId")]
        public virtual Employee GroupLeader { get; set; }

        // Гл_стр 
        [ForeignKey("MainBuilderId")]
        public virtual Employee MainBuilder { get; set; }

        // Дата_ред
        public DateTime? EditedDate { get; set; }

        public int? Signed1Id { get; set; }
        public int? Signed2Id { get; set; }

        public DateTime? IssuedDate { get; set; }

        public int? NumOfVolumes { get; set; }
        public float? SafetyCoeff { get; set; }
        public float? OperatingTemp { get; set; }
        public int? OperatingZone { get; set; }
        public int? GasGroup { get; set; }
        public int? Aggressiveness { get; set; }
        public int? Material { get; set; }
        public string PaintworkType { get; set; }
        public string Note { get; set; }
        public int? FireHazardCategoryId { get; set; }
        public int? HighTensileBolts { get; set; }
        public Boolean? P_transport { get; set; }
        public Boolean? P_site { get; set; }
        public Boolean? Xcnd { get; set; }
        public string Text_3d_estimate { get; set; }
        public string AddVolumes { get; set; }
        public float? VmpWeight { get; set; }
        public int? Impl_3d_estimate { get; set; }
    }
}
