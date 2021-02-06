using System.ComponentModel.DataAnnotations;

namespace Personnel.Dtos
{
    public class PositionRequest
    {
        [Required]
        [MaxLength(30)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LongName { get; set; }
    }
}