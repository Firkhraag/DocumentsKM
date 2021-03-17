using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Марка стали
    public class Steel
    {
        [Key]
        public Int16 Id { get; set; }

        // Наименование
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // ГОСТ
        [Required]
        [MaxLength(50)]
        public string Standard { get; set; }

        // Прочность
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? Strength { get; set; }
    }
}
