using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class Steel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Standard { get; set; }

        public int? Strength { get; set; }
    }
}
