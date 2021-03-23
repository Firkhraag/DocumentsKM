using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionElementUpdateRequest
    {
        public Int32? ProfileId { get; set; }

        public Int16? SteelId { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public float? Length { get; set; }

        public ConstructionElementUpdateRequest()
        {
            ProfileId = null;
            SteelId = null;
            Length = null;
        }
    }
}
