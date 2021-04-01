using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkCreateRequest
    {
        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public Int16 DepartmentId { get; set; }

        public Int32? ChiefSpecialistId { get; set; }
        public Int32? GroupLeaderId { get; set; }
        public Int32? NormContrId { get; set; }

        public MarkCreateRequest()
        {
            ChiefSpecialistId = null;
            GroupLeaderId = null;
            NormContrId = null;
        }
    }
}
