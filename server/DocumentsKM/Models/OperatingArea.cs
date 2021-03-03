using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Зона эксплуатации
    public class OperatingArea
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
