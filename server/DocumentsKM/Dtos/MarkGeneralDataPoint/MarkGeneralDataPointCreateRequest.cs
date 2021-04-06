using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkGeneralDataPointCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Text { get; set; }
    }
}
