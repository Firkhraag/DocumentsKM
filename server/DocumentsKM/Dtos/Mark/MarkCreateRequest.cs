using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkCreateRequest
    {
        [Required]
        public int SubnodeId { get; set; }

        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public int? ChiefSpecialistId { get; set; }
        public int? GroupLeaderId { get; set; }

        [Required]
        public int MainBuilderId { get; set; }

        public MarkCreateRequest()
        {
            ChiefSpecialistId = null;
            GroupLeaderId = null;
        }
    }
}
