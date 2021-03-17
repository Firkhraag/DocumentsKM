using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class DocCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Form { get; set; }

        [Required]
        public Int16 TypeId { get; set; }

        [Required]
        public Int32 CreatorId { get; set; }

        public Int32? InspectorId { get; set; }
        public Int32? NormContrId { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? ReleaseNum { get; set; }
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? NumOfPages { get; set; }

        [MaxLength(255)]
        public string Note { get; set; }

        public DocCreateRequest()
        {
            InspectorId = null;
            NormContrId = null;
        }
    }
}
