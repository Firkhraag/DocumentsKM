namespace DocumentsKM.Dtos
{
    public class ConstructionBoltUpdateRequest
    {
        public int? DiameterId { get; set; }

        public int? Packet { get; set; }

        public int? Num { get; set; }

        public int? NutNum { get; set; }

        public int? WasherNum { get; set; }

        public ConstructionBoltUpdateRequest()
        {
            DiameterId = null;
            Packet = null;
            Num = null;
            NutNum = null;
            WasherNum = null;
        }
    }
}
