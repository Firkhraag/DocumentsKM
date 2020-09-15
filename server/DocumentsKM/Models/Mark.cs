using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Model
{
    public class Mark
    {
        // Id_Марки
        [Key]
        public ulong Id { get; set; }

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

        [Required]
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        // Гл_спец
        [ForeignKey("EmployeeId")]
        public Employee ChiefSpecialist { get; set; }

        // Рук_гр 
        [ForeignKey("EmployeeId")]
        public Employee GroupLeader { get; set; }

        // Гл_стр 
        [Required]
        [ForeignKey("EmployeeId")]
        public Employee MainBulder { get; set; }

        // Исп1
        [ForeignKey("EmployeeId")]
        public Employee AgreedWorker1 { get; set; }

        // Исп2
        [ForeignKey("EmployeeId")]
        public Employee AgreedWorker2 { get; set; }

        // Исп3
        [ForeignKey("EmployeeId")]
        public Employee AgreedWorker3 { get; set; }

        // Исп4
        [ForeignKey("EmployeeId")]
        public Employee AgreedWorker4 { get; set; }

        // Исп5
        [ForeignKey("EmployeeId")]
        public Employee AgreedWorker5 { get; set; }

        // Исп6
        [ForeignKey("EmployeeId")]
        public Employee AgreedWorker6 { get; set; }

        // Исп7
        [ForeignKey("EmployeeId")]
        public Employee AgreedWorker7 { get; set; }
    }
}
