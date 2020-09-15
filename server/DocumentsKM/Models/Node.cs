using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Model
{
    public class Node
    {
        // Узел
        [Key]
        public ulong Id { get; set; }

        // Проект
        [Required]
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        // КодУзла
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        // НазвУзла
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // НазвУзлаДоп
        [Required]
        [MaxLength(255)]
        public string AdditionalName { get; set; }

        // ГИП
        [Required]
        [ForeignKey("EmployeeId")]
        public Employee ChiefEngineer { get; set; }

        // АктивУзел
        [Required]
        [MaxLength(30)]
        public string ActiveNode { get; set; }

        // ДатаУзел
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}