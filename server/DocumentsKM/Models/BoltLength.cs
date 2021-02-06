using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class BoltLength
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("DiameterId")]
        public virtual BoltDiameter Diameter { get; set; }

        [Required]
        public int BoltLen { get; set; }

        [Required]
        public int ScrewLen { get; set; }

        [Required]
        public float BoltWeight { get; set; }
    }
}
