using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Данные для задания на смету
    public class EstimateTask
    {
        // Марка
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }
        [Key]
        public Int32 MarkId { get; set; }

        // Текст
        [Required]
        public string TaskText { get; set; }

        // Дополнительный текст
        public string AdditionalText { get; set; }

        // Сотрудник для согласования
        [ForeignKey("ApprovalEmployeeId")]
        public virtual Employee ApprovalEmployee { get; set; }
        public Int32? ApprovalEmployeeId { get; set; }
    }
}
