using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Mark
    {
        // Id_Марки
        [Key]
        public int Id { get; set; }

        // ОА_Подузел
        [Required]
        [ForeignKey("SubnodeId")]
        public virtual Subnode Subnode { get; set; }
        public int SubnodeId { get; set; }

        // КодМарки
        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        // НазвМарки
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

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
        // [DataType(DataType.Date)]
        public DateTime? EditedDate { get; set; }

        public int Signed1Id { get; set; }
        public int Signed2Id { get; set; }

        // [DataType(DataType.Date)]
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

        // // Id_Марки
        // [Key]
        // public int Id { get; set; }

        // // ОА_Подузел
        // [Required]
        // [ForeignKey("SubnodeId")]
        // public virtual Subnode Subnode { get; set; }
        // public Int16 SubnodeId { get; set; }

        // // КодМарки
        // [Required]
        // [MaxLength(40)]
        // public string Code { get; set; }

        // // НазвМарки
        // [Required]
        // [MaxLength(255)]
        // public string Name { get; set; }

        // // Код_отд
        // [Required]
        // [ForeignKey("DepartmentId")]
        // public virtual Department Department { get; set; }

        // // Гл_спец
        // [ForeignKey("ChiefSpecialistId")]
        // public virtual Employee ChiefSpecialist { get; set; }

        // // Рук_гр 
        // [ForeignKey("GroupLeaderId")]
        // public virtual Employee GroupLeader { get; set; }

        // // Гл_стр 
        // [ForeignKey("MainBuilderId")]
        // public virtual Employee MainBuilder { get; set; }

        // // Дата_ред
        // [DataType(DataType.Date)]
        // public DateTime EditDate { get; set; }

        // public Int16 Signed1Id { get; set; }
        // public Int16 Signed2Id { get; set; }

        // [DataType(DataType.Date)]
        // public DateTime IssueDate { get; set; }

        // public Int16 NumOfVolumes { get; set; }
        // public float SafetyCoeff { get; set; }
        // public float OperatingTemp { get; set; }
        // public Int16 OperatingZone { get; set; }
        // public Int16 GasGroup { get; set; }
        // public Int16 Aggressiveness { get; set; }
        // public Int16 Material { get; set; }
        // public string PaintworkType { get; set; }
        // public string Note { get; set; }
        // public Int16 FireHazardCategoryId { get; set; }
        // public Int16 HighTensileBolts { get; set; }
        // public Boolean PTransport { get; set; }
        // public Boolean PSite { get; set; }
        // public Boolean Xcnd { get; set; }
        // public string Text_3d_estimate { get; set; }
        // public string AddVolumes { get; set; }
        // public float VmpWeight { get; set; }
        // public int Impl_3d_estimate { get; set; }
    }
}
