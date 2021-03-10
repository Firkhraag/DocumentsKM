using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class DefaultValuesUpdateRequest
    {
        public int? DepartmentId { get; set; }
        public int? CreatorId { get; set; }
        public int? InspectorId { get; set; }
        public int? NormContrId { get; set; }

        public DefaultValuesUpdateRequest()
        {
            DepartmentId = null;
            CreatorId = null;
            InspectorId = null;
            NormContrId = null;
        }
    }
}
