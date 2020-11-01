using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }

        // [Key]
        // public Int16 Id { get; set; }

        // [Required]
        // [MaxLength(50)]
        // public string Name { get; set; }

        // [Required]
        // [ForeignKey("DepartmentId")]
        // public virtual Department Department { get; set; }

        // [ForeignKey("PositionId")]
        // public virtual Position Position { get; set; }
    }
}
