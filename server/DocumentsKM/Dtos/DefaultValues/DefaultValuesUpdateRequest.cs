using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class DefaultValuesUpdateRequest
    {
        public Int16? DepartmentId { get; set; }
        public Int32? CreatorId { get; set; }
        public Int32? InspectorId { get; set; }
        public Int32? NormContrId { get; set; }

        public DefaultValuesUpdateRequest()
        {
            DepartmentId = null;
            CreatorId = null;
            InspectorId = null;
            NormContrId = null;
        }
    }
}
