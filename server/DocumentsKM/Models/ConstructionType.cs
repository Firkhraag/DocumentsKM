using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Вид конструкции (спр.)
    public class ConstructionType
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
