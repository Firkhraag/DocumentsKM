using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class GeneralDataSectionCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
