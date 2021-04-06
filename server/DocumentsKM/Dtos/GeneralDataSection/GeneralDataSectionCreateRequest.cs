using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class GeneralDataSectionCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
