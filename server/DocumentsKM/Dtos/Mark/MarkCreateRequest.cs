using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkCreateRequest
    {
        [Required]
        public Int32 SubnodeId { get; set; }

        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public Int16 DepartmentId { get; set; }

        public Int32? ChiefSpecialistId { get; set; }
        public Int32? GroupLeaderId { get; set; }

        [Required]
        public Int32 MainBuilderId { get; set; }

        public MarkCreateRequest()
        {
            ChiefSpecialistId = null;
            GroupLeaderId = null;
        }
    }
}
