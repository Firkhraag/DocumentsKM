using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class StandardConstructionCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Num { get; set; }

        [MaxLength(10)]
        public string Sheet { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Weight { get; set; }
    }
}
