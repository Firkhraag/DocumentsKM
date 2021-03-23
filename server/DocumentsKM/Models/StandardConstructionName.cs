using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Типовое наименование типовой конструкции
    public class StandardConstructionName
    {
        [Key]
        public Int16 Id { get; set; }

        // Наименование
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
