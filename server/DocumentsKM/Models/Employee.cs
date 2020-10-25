using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Employee
    {
        // таб_ном
        [Key]
        public int Id { get; set; }

        // фио
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        // ДатаПриема
        [Required]
        [DataType(DataType.Date)]
        public DateTime RecruitedDate { get; set; }

        // ДатаУвольнения
        [DataType(DataType.Date)]
        public DateTime FiredDate { get; set; }

        // ОтделКод, ОтделИмя
        [Required]
        [ForeignKey("DepartmentNumber")]
        public virtual Department Department { get; set; }

        // ДолжностьКод, ДолжностьИмя
        [ForeignKey("PositionCode")]
        public virtual Position Position { get; set; }
    }
}
