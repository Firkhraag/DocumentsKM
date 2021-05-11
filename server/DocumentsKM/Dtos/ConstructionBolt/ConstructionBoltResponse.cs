using System;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class ConstructionBoltResponse
    {
        public Int32 Id { get; set; }
        public BoltDiameter Diameter { get; set; }
        public Int16 Packet { get; set; }
        public Int16 Num { get; set; }
        public Int16 NutNum { get; set; }
        public Int16 WasherNum { get; set; }
    }
}
