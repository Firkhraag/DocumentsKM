using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Элемент вида конструкции
    public class ConstructionElement
    {
        [Key]
        public Int32 Id { get; set; }

        // Вид конструкции
        [Required]
        [ForeignKey("ConstructionId")]
        public virtual Construction Construction { get; set; }

        // Профиль
        [Required]
        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }

        // Марка стали
        [Required]
        [ForeignKey("SteelId")]
        public virtual Steel Steel { get; set; }

        // Длина
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Length { get; set; }

        // Арифметическое выражение
        [MaxLength(255)]
        public string ArithmeticExpression { get; set; }
    }
}
