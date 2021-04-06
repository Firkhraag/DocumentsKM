using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkGeneralDataPointUpdateRequest
    {
        [MaxLength(255)]
        [MinLength(1)]
        public string Text { get; set; }
        public Int16? OrderNum { get; set; }

        public MarkGeneralDataPointUpdateRequest()
        {
            Text = null;
            OrderNum = null;
        }
    }
}
