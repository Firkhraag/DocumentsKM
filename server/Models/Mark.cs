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
        [ForeignKey("SubnodeFK")]
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
        [ForeignKey("DepartmentFK")]
        public Department Department { get; set; }

        // Гл_спец
        [ForeignKey("EmployeeFK")]
        public Employee ChiefSpecialist { get; set; }

        // Рук_гр 
        [ForeignKey("EmployeeFK")]
        public Employee GroupLeader { get; set; }

        // Гл_стр 
        [Required]
        [ForeignKey("EmployeeFK")]
        public Employee MainBulder { get; set; }

        // Исп1
        [ForeignKey("EmployeeFK")]
        public Employee ApprovalSpecialist1 { get; set; }

        // Исп2
        [ForeignKey("EmployeeFK")]
        public Employee ApprovalSpecialist2 { get; set; }

        // Исп3
        [ForeignKey("EmployeeFK")]
        public Employee ApprovalSpecialist3 { get; set; }

        // Исп4
        [ForeignKey("EmployeeFK")]
        public Employee ApprovalSpecialist4 { get; set; }

        // Исп5
        [ForeignKey("EmployeeFK")]
        public Employee ApprovalSpecialist5 { get; set; }

        // Исп6
        [ForeignKey("EmployeeFK")]
        public Employee ApprovalSpecialist6 { get; set; }

        // Исп7
        [ForeignKey("EmployeeFK")]
        public Employee ApprovalSpecialist7 { get; set; }
    }
}
