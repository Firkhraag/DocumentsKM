using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class StandardConstructionCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public int? Num { get; set; }

        [MaxLength(10)]
        public string Sheet { get; set; }

        public float? Weight { get; set; }
    }
}
