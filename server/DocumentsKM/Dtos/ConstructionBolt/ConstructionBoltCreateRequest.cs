using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionBoltCreateRequest
    {
        [Required]
        public int DiameterId { get; set; }

        [Required]
        public int Packet { get; set; }

        [Required]
        public int Num { get; set; }

        [Required]
        public int NutNum { get; set; }

        [Required]
        public int WasherNum { get; set; }
    }
}
