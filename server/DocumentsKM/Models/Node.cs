using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Узел
    public class Node
    {
        [Key]
        public Int32 Id { get; set; }

        // Проект
        [Required]
        public Int32 ProjectId { get; set; }

        // Код
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        // Название
        [MaxLength(255)]
        public string Name { get; set; }

        // ГИП
        [MaxLength(255)]
        public string ChiefEngineerName { get; set; }
    }
}
