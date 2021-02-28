using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Контроль плотности сварных швов
    public class WeldingControl
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
