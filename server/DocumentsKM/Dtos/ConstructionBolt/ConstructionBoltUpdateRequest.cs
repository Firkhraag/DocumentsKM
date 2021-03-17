using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionBoltUpdateRequest
    {
        public Int16? DiameterId { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? Packet { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? Num { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? NutNum { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? WasherNum { get; set; }

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
