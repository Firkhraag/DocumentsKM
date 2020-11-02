using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class SheetUpdateRequest
    {
        public int? Num { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public float? Form { get; set; }
        public int? CreatorId { get; set; }
        public int? InspectorId { get; set; }
        public int? NormContrId { get; set; }
        [MaxLength(255)]
        public string Note { get; set; }

        public SheetUpdateRequest()
        {
            Num = null;
            Name = null;
            Form = null;
            CreatorId = null;
            InspectorId = null;
            NormContrId = null;
            Note = null;
        }
    }
}
