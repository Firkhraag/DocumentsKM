using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class DocCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public float Form { get; set; }

        [Required]
        public int TypeId { get; set; }

        public int? CreatorId { get; set; }
        public int? InspectorId { get; set; }
        public int? NormContrId { get; set; }

        public int? ReleaseNum { get; set; }
        public int? NumOfPages { get; set; }

        [MaxLength(255)]
        public string Note { get; set; }

        public DocCreateRequest()
        {
            CreatorId = null;
            InspectorId = null;
            NormContrId = null;
        }
    }
}
