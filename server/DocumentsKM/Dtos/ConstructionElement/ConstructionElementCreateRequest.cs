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
        [Range(0.0f, 10000.0f, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Length { get; set; }
    }
}
