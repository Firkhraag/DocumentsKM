using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Согласование марки
    public class MarkApproval
    {
        [Key]
        public int Id { get; set; }

        // Марка
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }
        public int MarkId { get; set; }

        // Сотрудник
        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
