using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Должность
    public class Position
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }
    }
}
