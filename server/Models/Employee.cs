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
        [ForeignKey("DepartmentFK")]
        public Department Department { get; set; }

        // ДолжностьКод, ДолжностьИмя
        [ForeignKey("PositionFK")]
        public Position Position { get; set; }

        // Телефон
        [MaxLength(50)]
        [Phone]
        public string PhoneNumber { get; set; }

        // Столовая
        [Required]
        public bool HasCanteen { get; set; }

        // Вид_отпуска
        [Required]
        public int VacationType { get; set; }

        // Дата_нач
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }

        // Дата_кон
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
