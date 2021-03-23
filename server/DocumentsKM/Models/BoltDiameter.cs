using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Диаметр болта
    public class BoltDiameter
    {
        [Key]
        public Int16 Id { get; set; }

        // Диаметр
        [Required]
        public Int16 Diameter { get; set; }

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
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 WasherThickness { get; set; }

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
