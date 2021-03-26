using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkGeneralDataSectionCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
