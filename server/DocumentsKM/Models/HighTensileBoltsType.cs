using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Высокопрочные болты
    public class HighTensileBoltsType
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
