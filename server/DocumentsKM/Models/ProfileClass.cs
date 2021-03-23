using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Вид профиля
    public class ProfileClass
    {
        [Key]
        public Int16 Id { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Примечание
        [MaxLength(255)]
        public string Note { get; set; }
    }
}
