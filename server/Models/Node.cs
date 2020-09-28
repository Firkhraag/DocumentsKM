using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Node
    {
        // Узел
        [Key]
        public int Id { get; set; }

        // Проект
        [Required]
        [ForeignKey("ProjectFK")]
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
        [ForeignKey("EmployeeFK")]
        public Employee ChiefEngineer { get; set; }

        // АктивУзел
        [Required]
        [MaxLength(30)]
        public string ActiveNode { get; set; }

        // ДатаУзел
        [Required]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
    }
}