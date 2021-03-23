using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionUpdateRequest
    {
        [MaxLength(255)]
        public string Name { get; set; }

        public Int16? TypeId { get; set; }

        public Int16? SubtypeId { get; set; }

        [MaxLength(10)]
        public string Valuation { get; set; }

        [MaxLength(20)]
        public string StandardAlbumCode { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? NumOfStandardConstructions { get; set; }

        public bool? HasEdgeBlunting { get; set; }

        public bool? HasDynamicLoad { get; set; }

        public bool? HasFlangedConnections { get; set; }

        public Int16? WeldingControlId { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public float? PaintworkCoeff { get; set; }

        public ConstructionUpdateRequest()
        {
            Name = null;
            TypeId = null;
            SubtypeId = null;
            Valuation = null;
            StandardAlbumCode = null;
            NumOfStandardConstructions = null;
            HasEdgeBlunting = null;
            HasDynamicLoad = null;
            HasFlangedConnections = null;
            WeldingControlId = null;
            PaintworkCoeff = null;
        }
    }
}
