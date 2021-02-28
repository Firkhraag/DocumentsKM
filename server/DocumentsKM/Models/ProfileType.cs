using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Тип профиля
    public class ProfileType
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
