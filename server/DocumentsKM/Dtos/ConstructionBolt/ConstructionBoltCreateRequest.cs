using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionBoltCreateRequest
    {
        [Required]
        public int DiameterId { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Packet { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Num { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int NutNum { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int WasherNum { get; set; }
    }
}
