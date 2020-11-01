using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        // [Key]
        // public Int16 Id { get; set; }

        // [Required]
        // [MaxLength(30)]
        // public string Name { get; set; }
    }
}
