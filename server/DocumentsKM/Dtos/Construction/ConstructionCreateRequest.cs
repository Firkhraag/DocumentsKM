using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public Int16 TypeId { get; set; }

        public Int16? SubtypeId { get; set; }

        [MaxLength(10)]
        public string Valuation { get; set; }

        [MaxLength(20)]
        public string StandardAlbumCode { get; set; }

        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 NumOfStandardConstructions { get; set; }

        [Required]
        public bool HasEdgeBlunting { get; set; }

        [Required]
        public bool HasDynamicLoad { get; set; }

        [Required]
        public bool HasFlangedConnections { get; set; }

        [Required]
        public Int16 WeldingControlId { get; set; }

        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public float PaintworkCoeff { get; set; }

        public ConstructionCreateRequest()
        {
            SubtypeId = null;
            Valuation = null;
            StandardAlbumCode = null;
        }
    }
}
