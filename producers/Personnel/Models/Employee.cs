using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personnel.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        [Required]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
    }
}
