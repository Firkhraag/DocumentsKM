using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Материал конструкций
    public class ConstructionMaterial
    {
        [Key]
        public Int16 Id { get; set; }

        // Название
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
