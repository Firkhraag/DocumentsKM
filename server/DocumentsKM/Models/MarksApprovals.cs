using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class MarksApprovals
    {
        [Required]
        [ForeignKey("MarkId")]
        public Mark Mark { get; set; }

        [Required]
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
