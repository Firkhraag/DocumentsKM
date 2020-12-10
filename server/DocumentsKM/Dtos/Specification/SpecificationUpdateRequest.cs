using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class SpecificationUpdateRequest
    {
        [MaxLength(255)]
        public string Note { get; set; }
        public bool? IsCurrent { get; set; }

        public SpecificationUpdateRequest()
        {
            Note = null;
            IsCurrent = null;
        }
    }
}
