using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class ProfileType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
