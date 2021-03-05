using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Грунтовка
    public class Primer
    {
        [Key]
        public int Id { get; set; }

        // Номер группы
        [Required]
        public int GroupNum { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Можно грунтовать
        [Required]
        public bool CanBePrimed { get; set; }

        // Приоритет
        [Required]
        public int Priority { get; set; }
    }
}
