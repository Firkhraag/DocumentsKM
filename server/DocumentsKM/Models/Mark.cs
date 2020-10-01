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
        public Subnode Subnode { get; set; }

        // КодМарки
        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        // CREATE UNIQUE INDEX <name> ON (ОА_Подузел, КодМарки)

        // ДопКод
        [Required]
        [MaxLength(50)]
        public string AdditionalCode { get; set; }

        // НазвМарки
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Код_отд
        [Required]
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        // Гл_спец
        [ForeignKey("ChiefSpecialistId")]
        public Employee ChiefSpecialist { get; set; }

        // Рук_гр 
        [ForeignKey("GroupLeaderId")]
        public Employee GroupLeader { get; set; }

        // Гл_стр 
        [Required]
        [ForeignKey("MainBulderId")]
        public Employee MainBulder { get; set; }

        // Исп1
        [ForeignKey("ApprovalSpecialist1Id")]
        public Employee ApprovalSpecialist1 { get; set; }

        // Исп2
        [ForeignKey("ApprovalSpecialist2Id")]
        public Employee ApprovalSpecialist2 { get; set; }

        // Исп3
        [ForeignKey("ApprovalSpecialist3Id")]
        public Employee ApprovalSpecialist3 { get; set; }

        // Исп4
        [ForeignKey("ApprovalSpecialist4Id")]
        public Employee ApprovalSpecialist4 { get; set; }

        // Исп5
        [ForeignKey("ApprovalSpecialist5Id")]
        public Employee ApprovalSpecialist5 { get; set; }

        // Исп6
        [ForeignKey("ApprovalSpecialist6Id")]
        public Employee ApprovalSpecialist6 { get; set; }

        // Исп7
        [ForeignKey("ApprovalSpecialist7Id")]
        public Employee ApprovalSpecialist7 { get; set; }
    }
}
