using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Раздел общих указаний
    public class GeneralDataSection
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Номер
        [Required]
        public int OrderNum { get; set; }
    }
}
