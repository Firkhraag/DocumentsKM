using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class DocCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Form { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public int CreatorId { get; set; }

        public int? InspectorId { get; set; }
        public int? NormContrId { get; set; }

        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int? ReleaseNum { get; set; }
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int? NumOfPages { get; set; }

        [MaxLength(255)]
        public string Note { get; set; }

        public DocCreateRequest()
        {
            InspectorId = null;
            NormContrId = null;
        }
    }
}
