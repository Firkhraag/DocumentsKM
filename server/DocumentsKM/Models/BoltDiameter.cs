using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Диаметр болта
    public class BoltDiameter
    {
        [Key]
        public int Id { get; set; }

        // Диаметр
        [Required]
        public int Diameter { get; set; }

        // Вес гайки
        [Required]
        public float NutWeight { get; set; }

        // Сталь шайбы
        [Required]
        [MaxLength(50)]
        public string WasherSteel { get; set; }

        // Вес шайбы
        [Required]
        public float WasherWeight { get; set; }

        // Толщина шайбы
        [Required]
        public int WasherThickness { get; set; }

        // Нормативный документ на болты
        [Required]
        [MaxLength(50)]
        public string BoltTechSpec { get; set; }

        // Класс прочности болтов
        [Required]
        [MaxLength(50)]
        public string StrengthClass { get; set; }

        // Нормативный документ на гайки
        [Required]
        [MaxLength(50)]
        public string NutTechSpec { get; set; }

        // Нормативный документ на шайбы
        [Required]
        [MaxLength(50)]
        public string WasherTechSpec { get; set; }
    }
}
