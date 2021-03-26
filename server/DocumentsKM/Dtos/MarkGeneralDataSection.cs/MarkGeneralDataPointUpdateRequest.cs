using System;

namespace DocumentsKM.Dtos
{
    public class MarkGeneralDataSectionUpdateRequest
    {
        public string Name { get; set; }
        public Int16? OrderNum { get; set; }

        public MarkGeneralDataSectionUpdateRequest()
        {
            Name = null;
            OrderNum = null;
        }
    }
}
