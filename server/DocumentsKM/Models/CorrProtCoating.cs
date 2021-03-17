using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Покрытие антикоррозионной защиты
    public class CorrProtCoating
    {
        [Key]
        public Int16 Id { get; set; }

        // Тип лакокрасочного материала
        [Required]
        [ForeignKey("PaintworkTypeId")]
        public virtual PaintworkType PaintworkType { get; set; }

        // Группа покраски
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 PaintworkGroup { get; set; }

        // Стойкость
        [Required]
        [ForeignKey("PaintworkFastnessId")]
        public virtual PaintworkFastness PaintworkFastness { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Число слоев
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? PaintworkNumOfLayers { get; set; }

        // Группа грунтовки
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 PrimerGroup { get; set; }

        // Можно красить
        [Required]
        public bool CanBePainted { get; set; }

        // Приоритет
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Priority { get; set; }
    }
}
