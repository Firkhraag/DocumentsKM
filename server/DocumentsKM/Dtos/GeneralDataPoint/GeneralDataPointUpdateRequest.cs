using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class GeneralDataPointUpdateRequest
    {
        [MaxLength(255)]
        [MinLength(1)]
        public string Text { get; set; }
        public Int16? OrderNum { get; set; }

        public GeneralDataPointUpdateRequest()
        {
            Text = null;
            OrderNum = null;
        }
    }
}
