using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Сотрудника
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        // Имя
        [Required]
        [MaxLength(50)]
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
