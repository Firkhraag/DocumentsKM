using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionBoltUpdateRequest
    {
        public int? DiameterId { get; set; }

        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int? Packet { get; set; }

        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int? Num { get; set; }

        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int? NutNum { get; set; }

        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
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
