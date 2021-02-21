using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int TypeId { get; set; }

        public int? SubtypeId { get; set; }

        [MaxLength(10)]
        public string Valuation { get; set; }

        [MaxLength(20)]
        public string StandardAlbumCode { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int NumOfStandardConstructions { get; set; }

        [Required]
        public bool HasEdgeBlunting { get; set; }

        [Required]
        public bool HasDynamicLoad { get; set; }

        [Required]
        public bool HasFlangedConnections { get; set; }

        [Required]
        public int WeldingControlId { get; set; }

        [Required]
        [Range(0.0f, 10000.0f, ErrorMessage = "Value should be greater than or equal to 0")]
        public float PaintworkCoeff { get; set; }

        public ConstructionCreateRequest()
        {
            SubtypeId = null;
            Valuation = null;
            StandardAlbumCode = null;
        }
    }
}
