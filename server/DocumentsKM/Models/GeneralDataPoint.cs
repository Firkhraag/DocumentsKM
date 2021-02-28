using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Пункт общих указаний пользователя
    public class GeneralDataPoint
    {
        [Key]
        public int Id { get; set; }

        // Пользователь
        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Раздел
        [Required]
        [ForeignKey("SectionId")]
        public virtual GeneralDataSection Section { get; set; }

        // Текст
        [Required]
        public string Text { get; set; }

        // Номер
        [Required]
        public int OrderNum { get; set; }
    }
}
