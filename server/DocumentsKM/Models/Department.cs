using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Отдел
    public class Department
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
