using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class GeneralDataPointCreateRequest
    {
        [Required]
        public string Text { get; set; }

        // [Required]
        // public int OrderNum { get; set; }
    }
}
