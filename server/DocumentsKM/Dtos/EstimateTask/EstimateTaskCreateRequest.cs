using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class EstimateTaskCreateRequest
    {
        [Required]
        public string TaskText { get; set; }

        public string AdditionalText { get; set; }

        public int? ApprovalEmployeeId { get; set; }

        public EstimateTaskCreateRequest()
        {
            AdditionalText = null;
            ApprovalEmployeeId = null;
        }
    }
}
