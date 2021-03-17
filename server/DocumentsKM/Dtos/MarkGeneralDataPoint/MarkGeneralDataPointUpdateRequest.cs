using System;

namespace DocumentsKM.Dtos
{
    public class MarkGeneralDataPointUpdateRequest
    {
        public string Text { get; set; }
        public Int16? OrderNum { get; set; }

        public MarkGeneralDataPointUpdateRequest()
        {
            Text = null;
            OrderNum = null;
        }
    }
}
