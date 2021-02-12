using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class DocUpdateRequest
    {
        [MaxLength(255)]
        public string Name { get; set; }
        [Range(0.0f, 10000.0f, ErrorMessage = "Value should be greater than or equal to 0")]
        public float? Form { get; set; }
        public int? CreatorId { get; set; }
        public int? InspectorId { get; set; }
        public int? NormContrId { get; set; }

        public int? TypeId { get; set; }

        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int? ReleaseNum { get; set; }
        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int? NumOfPages { get; set; }
        [MaxLength(255)]
        public string Note { get; set; }

        public DocUpdateRequest()
        {
            Name = null;
            Form = null;
            CreatorId = null;
            InspectorId = null;
            NormContrId = null;

            TypeId = null;

            ReleaseNum = null;
            NumOfPages = null;
            Note = null;
        }
    }
}
