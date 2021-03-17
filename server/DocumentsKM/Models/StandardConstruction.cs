using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Типовая конструкция
    public class StandardConstruction
    {
        [Key]
        public Int16 Id { get; set; }

        // Выпуск спецификации
        [Required]
        [ForeignKey("SpecificationId")]
        public virtual Specification Specification { get; set; }

        // Наименование
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Номер
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Num { get; set; }

        // Лист
        [MaxLength(10)]
        public string Sheet { get; set; }

        // Вес
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Weight { get; set; }
    }
}
