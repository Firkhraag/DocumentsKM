using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Ссылочный документ (спр.)
    public class LinkedDoc
    {
        [Key]
        public Int16 Id { get; set; }

        // Шифр
        [Required]
        [MaxLength(4)]
        public string Code { get; set; }

        // Вид
        [Required]
        [ForeignKey("TypeId")]
        public virtual LinkedDocType Type { get; set; }

        // Обозначение
        [Required]
        [MaxLength(40)]
        public string Designation { get; set; }

        // Наименование
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
