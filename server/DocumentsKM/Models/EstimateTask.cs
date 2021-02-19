using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class EstimateTask
    {
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }
        [Key]
        public int MarkId { get; set; }

        [Required]
        public string TaskText { get; set; }

        public string AdditionalText { get; set; }

        [ForeignKey("ApprovalEmployeeId")]
        public virtual Employee ApprovalEmployee { get; set; }
        public int? ApprovalEmployeeId { get; set; }
    }
}
