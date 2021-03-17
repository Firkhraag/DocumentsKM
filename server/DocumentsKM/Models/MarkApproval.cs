using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Согласование марки
    public class MarkApproval
    {
        [Key]
        public Int32 Id { get; set; }

        // Марка
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }
        public Int32 MarkId { get; set; }

        // Сотрудник
        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public Int32 EmployeeId { get; set; }
    }
}
