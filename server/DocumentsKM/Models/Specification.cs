using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Выпуск спецификации
    public class Specification
    {
        [Key]
        public Int32 Id { get; set; }

        // Марка
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        // Номер
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Num { get; set; }

        // Текущий
        [Required]
        public bool IsCurrent { get; set; }

        // Текущий
        [Required]
        public bool IsIncluded { get; set; }

        // Примечание
        [MaxLength(255)]
        public string Note { get; set; }

        // Дата создания
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }

        // Виды конструкций
        public virtual IList<Construction> Constructions { get; set; } = new List<Construction>();

        // Типовые конструкции
        public virtual IList<StandardConstruction> StandardConstructions { get; set; } = new List<StandardConstruction>();
    }
}
