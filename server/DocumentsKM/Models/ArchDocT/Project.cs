using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class Project
    {
        // For now we will consider NOT NULL constraint for every field

        // Проект
        [Key]
        public ulong Id { get; set; }

        // ВидРаботы
        [Required]
        public uint Type { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // БазСерия
        [Required]
        [MaxLength(20)]
        public string BaseSeries { get; set; }
    }
}