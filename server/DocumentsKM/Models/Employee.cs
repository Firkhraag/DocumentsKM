using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Model
{
    public class Employee
    {
        // таб_ном
        [Key]
        public ulong Id { get; set; }

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
        public Department Department { get; set; }

        // ДолжностьКод, ДолжностьИмя
        [ForeignKey("PositionCode")]
        public Position Position { get; set; }

        // Телефон
        [MaxLength(50)]
        [Phone]
        public string PhoneNumber { get; set; }

        // Столовая
        [Required]
        public bool IsCanteen { get; set; }

        // Вид_отпуска
        [Required]
        public ulong VacationType { get; set; }

        // Дата_нач
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }

        // Дата_кон
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}