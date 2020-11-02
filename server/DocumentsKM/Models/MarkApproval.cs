using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class MarkApproval
    {
        // Марка
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }
        public int MarkId { get; set; }

        // Исп
        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
