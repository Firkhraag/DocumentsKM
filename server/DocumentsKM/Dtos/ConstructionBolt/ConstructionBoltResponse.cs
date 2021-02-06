using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class ConstructionBoltResponse
    {
        public int Id { get; set; }
        public BoltDiameter Diameter { get; set; }
        public int Packet { get; set; }
        public int Num { get; set; }
        public int NutNum { get; set; }
        public int WasherNum { get; set; }
    }
}
