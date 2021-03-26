using System;

namespace DocumentsKM.Dtos
{
    public class GeneralDataSectionUpdateRequest
    {
        public string Name { get; set; }
        public Int16? OrderNum { get; set; }

        public GeneralDataSectionUpdateRequest()
        {
            Name = null;
            OrderNum = null;
        }
    }
}
