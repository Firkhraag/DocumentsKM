using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkGeneralDataSectionCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
