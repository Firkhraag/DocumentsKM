using System.ComponentModel.DataAnnotations;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkRequest
    {
        [Required]
        public int SubnodeId { get; set; }
        // public SubnodeResponse Subnode { get; set; }

        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int DepartmentNumber { get; set; }
        // public DepartmentBaseResponse Department { get; set; }

        public int ChiefSpecialistId { get; set; }
        // public EmployeeBaseResponse ChiefSpecialist { get; set; }

        public int GroupLeaderId { get; set; }
        // public EmployeeBaseResponse GroupLeader { get; set; }

        [Required]
        public int MainBuilderId { get; set; }
        // public EmployeeBaseResponse MainBuilder { get; set; }

        public MarkRequest()
        {
            ChiefSpecialistId = -1;
            GroupLeaderId = -1;
        }
    }
}
