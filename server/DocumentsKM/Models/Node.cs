using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Узел
    public class Node
    {
        [Key]
        public Int16 Id { get; set; }

        // Проект
        [Required]
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        // Код
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // ГИП
        [Required]
        [ForeignKey("ChiefEngineerId")]
        public virtual Employee ChiefEngineer { get; set; }
    }
}
