using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Подвид конструкции
    public class ConstructionSubtype
    {
        [Key]
        public Int16 Id { get; set; }

        // Вид конструкции
        [Required]
        [ForeignKey("TypeId")]
        public virtual ConstructionType Type { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Расценка
        [Required]
        [MaxLength(10)]
        public string Valuation { get; set; }
    }
}
