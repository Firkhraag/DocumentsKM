using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkGeneralDataSectionUpdateRequest
    {
        [MaxLength(255)]
        [MinLength(1)]
        public string Name { get; set; }
        public Int16? OrderNum { get; set; }

        public MarkGeneralDataSectionUpdateRequest()
        {
            Name = null;
            OrderNum = null;
        }
    }
}
