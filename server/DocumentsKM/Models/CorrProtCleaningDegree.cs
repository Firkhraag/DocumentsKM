using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Степень очистки антикоррозионной защиты
    public class CorrProtCleaningDegree
    {
        [Key]
        public Int16 Id { get; set; }

        // Название
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
