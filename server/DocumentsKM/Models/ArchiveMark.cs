using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Марка
    public class ArchiveMark
    {
        // Название
        [MaxLength(255)]
        public string Name { get; set; }

        // Код
        [Required]
        [MaxLength(30)]
        public string Code { get; set; }

        // Отдел
        [Required]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
