using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Отдел
    public class Department
    {
        [Key]
        public Int16 Id { get; set; }

        // Код отдела
        [Required]
        [MaxLength(6)]
        public string Code { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Сокращенное название
        [Required]
        [MaxLength(50)]
        public string ShortName { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
