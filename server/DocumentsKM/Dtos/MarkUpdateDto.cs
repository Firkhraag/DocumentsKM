using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkUpdateDto
    {
        [Required]
        public ulong SubnodeId { get; set; }

        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string AdditionalCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public ulong DepartmentId { get; set; }

        [Required]
        public ulong DepartmentHeadId { get; set; }

        [Required]
        public ulong ChiefSpecialistId { get; set; }

        [Required]
        public ulong GroupLeaderId { get; set; }

        [Required]
        public ulong MainBulderId { get; set; }

        public string ExploitationT { get; set; }

        public ulong AgreedDepartment1 { get; set; }

        public ulong AgreedWorker1 { get; set; }

        public ulong AgreedDepartment2 { get; set; }

        public ulong AgreedWorker2 { get; set; }

        public ulong AgreedDepartment3 { get; set; }

        public ulong AgreedWorker3 { get; set; }

        public ulong AgreedDepartment4 { get; set; }

        public ulong AgreedWorker4 { get; set; }

        public ulong AgreedDepartment5 { get; set; }

        public ulong AgreedWorker5 { get; set; }

        public ulong AgreedDepartment6 { get; set; }

        public ulong AgreedWorker6 { get; set; }

        public ulong AgreedDepartment7 { get; set; }

        public ulong AgreedWorker7 { get; set; }
    }
}
