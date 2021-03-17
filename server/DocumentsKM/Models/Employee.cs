using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Сотрудник
    public class Employee
    {
        [Key]
        public Int32 Id { get; set; }

        // Полное имя
        [Required]
        [MaxLength(255)]
        public string Fullname { get; set; }

        // Фамилия
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        // Отдел
        [Required]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        // Должность
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
    }
}
