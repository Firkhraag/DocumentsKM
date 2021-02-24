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
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Packet { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Num { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int NutNum { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int WasherNum { get; set; }
    }
}
