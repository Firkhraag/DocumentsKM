using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionElementCreateRequest
    {
        [Required]
        public int ProfileId { get; set; }

        [Required]
        public int SteelId { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Length { get; set; }
    }
}
