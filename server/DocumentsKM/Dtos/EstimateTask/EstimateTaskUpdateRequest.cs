using System;

namespace DocumentsKM.Dtos
{
    public class EstimateTaskUpdateRequest
    {
        public string TaskText { get; set; }
        public string AdditionalText { get; set; }
        public Int32? ApprovalEmployeeId { get; set; }

        public EstimateTaskUpdateRequest()
        {
            TaskText = null;
            AdditionalText = null;
            ApprovalEmployeeId = null;
        }
    }
}
