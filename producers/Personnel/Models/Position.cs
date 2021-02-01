using System.ComponentModel.DataAnnotations;

namespace Personnel.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LongName { get; set; }
    }
}
