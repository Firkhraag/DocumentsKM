using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Дополнительные работы
    public class AdditionalWork
    {
        [Key]
        public Int32 Id { get; set; }

        // Марка
        [Required]
        public virtual Mark Mark { get; set; }

        // Исполнитель
        [Required]
        public virtual Employee Employee { get; set; }

        // Расчет
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Valuation { get; set; }

        // Заказ металла
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 MetalOrder { get; set; }
    }
}
