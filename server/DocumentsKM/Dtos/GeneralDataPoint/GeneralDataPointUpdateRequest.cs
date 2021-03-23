using System;

namespace DocumentsKM.Dtos
{
    public class GeneralDataPointUpdateRequest
    {
        public string Text { get; set; }
        public Int16? OrderNum { get; set; }

        public GeneralDataPointUpdateRequest()
        {
            Text = null;
            OrderNum = null;
        }
    }
}
