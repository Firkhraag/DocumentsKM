using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Группа газов
    public class GasGroup
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
