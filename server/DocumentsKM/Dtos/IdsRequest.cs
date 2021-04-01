using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class IdsRequest
    {
        [Required]
        public List<int> Ids { get; set; }
    }
}
