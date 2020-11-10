using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class DocUpdateRequest
    {
        public int? Num { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public float? Form { get; set; }
        public int? CreatorId { get; set; }
        public int? InspectorId { get; set; }
        public int? NormContrId { get; set; }

        public int? ReleaseNum { get; set; }
        public int? NumOfPages { get; set; }
        [MaxLength(255)]
        public string Note { get; set; }

        public DocUpdateRequest()
        {
            Num = null;
            Name = null;
            Form = null;
            CreatorId = null;
            InspectorId = null;
            NormContrId = null;

            ReleaseNum = null;
            NumOfPages = null;
            Note = null;
        }
    }
}
