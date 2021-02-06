using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionUpdateRequest
    {
        [MaxLength(255)]
        public string Name { get; set; }

        public int? TypeId { get; set; }

        public int? SubtypeId { get; set; }

        [MaxLength(10)]
        public string Valuation { get; set; }

        [MaxLength(20)]
        public string StandardAlbumCode { get; set; }

        public int? NumOfStandardConstructions { get; set; }

        public bool? HasEdgeBlunting { get; set; }

        public bool? HasDynamicLoad { get; set; }

        public bool? HasFlangedConnections { get; set; }

        public int? WeldingControlId { get; set; }

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
