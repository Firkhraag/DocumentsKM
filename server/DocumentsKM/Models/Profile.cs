using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ClassId")]
        public virtual ProfileClass Class { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(2)]
        public string Symbol { get; set; }

        [Required]
        public float Weight { get; set; }

        [Required]
        public float Area { get; set; }

        [Required]
        [ForeignKey("TypeId")]
        public virtual ProfileType Type { get; set; }

    }
}
