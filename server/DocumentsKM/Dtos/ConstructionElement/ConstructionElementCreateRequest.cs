using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionElementCreateRequest
    {
        [Required]
        public int ProfileClassId { get; set; }

        [Required]
        public int ProfileId { get; set; }

        [Required]
        public int SteelId { get; set; }

        [Required]
        public float Length { get; set; }
    }
}
