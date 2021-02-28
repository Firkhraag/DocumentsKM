using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Материал конструкций
    public class ConstructionMaterial
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
