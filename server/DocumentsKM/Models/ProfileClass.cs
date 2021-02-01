using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class ProfileClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Note { get; set; }
    }
}
