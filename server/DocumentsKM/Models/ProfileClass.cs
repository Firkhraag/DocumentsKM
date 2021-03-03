using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Вид профиля
    public class ProfileClass
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Примечание
        [MaxLength(255)]
        public string Note { get; set; }
    }
}
