using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class ConstructionBolt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ConstructionId")]
        public virtual Construction Construction { get; set; }

        [Required]
        [ForeignKey("DiameterId")]
        public virtual BoltDiameter Diameter { get; set; }

        [Required]
        public int Packet { get; set; }

        [Required]
        public int Num { get; set; }

        [Required]
        public int NutNum { get; set; }

        [Required]
        public int WasherNum { get; set; }
    }
}
