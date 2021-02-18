using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Construction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("SpecificationId")]
        public virtual Specification Specification { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("TypeId")]
        public virtual ConstructionType Type { get; set; }

        [ForeignKey("SubtypeId")]
        public virtual ConstructionSubtype Subtype { get; set; }
        public int? SubtypeId { get; set; }

        [MaxLength(10)]
        public string Valuation { get; set; }

        [MaxLength(20)]
        public string StandardAlbumCode { get; set; }

        [Required]
        public int NumOfStandardConstructions { get; set; }

        [Required]
        public bool HasEdgeBlunting { get; set; }

        [Required]
        public bool HasDynamicLoad { get; set; }

        [Required]
        public bool HasFlangedConnections { get; set; }

        [Required]
        [ForeignKey("WeldingControlId")]
        public virtual WeldingControl WeldingControl { get; set; }

        [Required]
        public float PaintworkCoeff { get; set; }

        public virtual IList<ConstructionElement> ConstructionElements { get; set; } = new List<ConstructionElement>();

        public virtual IList<ConstructionBolt> ConstructionBolts { get; set; } = new List<ConstructionBolt>();
    }
}
