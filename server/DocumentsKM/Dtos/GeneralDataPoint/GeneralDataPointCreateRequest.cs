using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class GeneralDataPointCreateRequest
    {
        [Required]
        public string Text { get; set; }

        // public int? OrderNum { get; set; }

        // public GeneralDataPointCreateRequest()
        // {
        //     OrderNum = null;
        // }
    }
}
