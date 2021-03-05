using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Not used now
    // Степень очистки антикоррозионной защиты
    public class CorrProtCleaningDegree
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
