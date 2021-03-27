using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class GeneralDataSectionCopyRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
