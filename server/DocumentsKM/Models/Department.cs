using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Model
{
    public class Department
    {
        // НомОтдела
        [Key]
        public ulong Number { get; set; }

        // НазваниеОтдела
        [Required]
        [MaxLength(140)]
        public string Name { get; set; }

        // НазваниеОтделаК
        [Required]
        [MaxLength(40)]
        public string ShortName { get; set; }

        // КодОтдела
        [Required]
        [MaxLength(5)]
        public string Code { get; set; }

        // Активность
        [Required]
        public bool IsActive { get; set; }

        // Производственный
        [Required]
        public bool IsIndustrial { get; set; }

        // Нач_отд 
        [Required]
        [ForeignKey("EmployeeId")]
        public Employee DepartmentHead { get; set; }

        public List<Employee> Employees { get; set; }
    }
}