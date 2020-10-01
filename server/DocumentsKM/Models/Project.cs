using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Project
    {
        // Проект
        [Key]
        public int Id { get; set; }

        // ВидРаботы
        [Required]
        public int Type { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // НазваниеДоп
        [Required]
        [MaxLength(255)]
        public string AdditionalName { get; set; }

        // БазСерия
        [Required]
        [MaxLength(20)]
        public string BaseSeries { get; set; }

        // Утвердил1
        [ForeignKey("Approved1Id")]
        public Employee Approved1 { get; set; }

        // Утвердил2
        [ForeignKey("Approved2Id")]
        public Employee Approved2 { get; set; }
    }
}
