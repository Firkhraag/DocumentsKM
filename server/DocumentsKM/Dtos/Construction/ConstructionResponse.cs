using System;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class ConstructionResponse
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public ConstructionType Type { get; set; }
        public ConstructionSubtype Subtype { get; set; }
        public string Valuation { get; set; }
        public string StandardAlbumCode { get; set; }
        public Int16 NumOfStandardConstructions { get; set; }
        public bool HasEdgeBlunting { get; set; }
        public bool HasDynamicLoad { get; set; }
        public bool HasFlangedConnections { get; set; }
        public WeldingControl WeldingControl { get; set; }
        public float PaintworkCoeff { get; set; }
    }
}
