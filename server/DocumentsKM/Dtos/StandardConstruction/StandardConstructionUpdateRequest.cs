using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Dtos
{
    public class StandardConstructionUpdateRequest
    {
        [MaxLength(255)]
        public string Name { get; set; }

        public int? Num { get; set; }

        [MaxLength(10)]
        public string Sheet { get; set; }

        public float? Weight { get; set; }

        public StandardConstructionUpdateRequest()
        {
            Name = null;
            Num = null;
            Sheet = null;
            Weight = null;
        }
    }
}
