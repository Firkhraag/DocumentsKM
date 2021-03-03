using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Профиль
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        // Вид профиля
        [Required]
        [ForeignKey("ClassId")]
        public virtual ProfileClass Class { get; set; }

        // Название
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        // Символ
        [MaxLength(2)]
        public string Symbol { get; set; }

        // Вес
        [Required]
        public float Weight { get; set; }

        // Площадь
        [Required]
        public float Area { get; set; }

        // Тип профиля
        [Required]
        [ForeignKey("TypeId")]
        public virtual ProfileType Type { get; set; }

    }
}
