using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionElementCreateRequest
    {
        [Required]
        public int ProfileClassId { get; set; }

        [Required]
        [MaxLength(30)]
        public string ProfileName { get; set; }

        [Required]
        [MaxLength(2)]
        public string Symbol { get; set; }

        [Required]
        public float Weight { get; set; }

        [Required]
        public float SurfaceArea { get; set; }

        [Required]
        public int ProfileTypeId { get; set; }

        [Required]
        public int SteelId { get; set; }

        [Required]
        public float Length { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
