using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class SheetCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public float Form { get; set; }

        public int DocTypeId { get; set; }

        public int? CreatorId { get; set; }
        public int? InspectorId { get; set; }
        public int? NormContrId { get; set; }

        // public int ReleaseNum { get; set; }

        // public int NumOfSheets { get; set; }

        [MaxLength(255)]
        public string Note { get; set; }

        public SheetCreateRequest()
        {
            CreatorId = null;
            InspectorId = null;
            NormContrId = null;
        }
    }
}
