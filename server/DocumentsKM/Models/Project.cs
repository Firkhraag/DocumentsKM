using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Model
{
    public class Project
    {
        // Проект
        [Key]
        public ulong Id { get; set; }

        // ВидРаботы
        [Required]
        public uint Type { get; set; }

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
        [Required]
        [MaxLength(20)]
        [ForeignKey("EmployeeId")]
        public Employee Approved1 { get; set; }

        // Утвердил2
        [Required]
        [MaxLength(20)]
        [ForeignKey("EmployeeId")]
        public Employee Approved2 { get; set; }

        // // Должн1
        // [Required]
        // [MaxLength(30)]
        // public string Position1 { get; set; }

        // // Утвердил1
        // [Required]
        // [MaxLength(20)]
        // public string Approved1 { get; set; }

        // // Должн2
        // [Required]
        // [MaxLength(30)]
        // public string Position2 { get; set; }

        // // Утвердил2
        // [Required]
        // [MaxLength(20)]
        // public string Approved2 { get; set; }

        // public List<Node> Nodes { get; set; }
    }
}
