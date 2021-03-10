using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Not used now
    // Покрытие антикоррозионной защиты
    public class CorrProtCoating
    {
        [Key]
        public int Id { get; set; }

        // Тип лакокрасочного материала
        [Required]
        [ForeignKey("PaintworkTypeId")]
        public virtual PaintworkType PaintworkType { get; set; }

        // Группа покраски
        [Required]
        public int PaintworkGroup { get; set; }

        // Стойкость
        [Required]
        [ForeignKey("PaintworkFastnessId")]
        public virtual PaintworkFastness PaintworkFastness { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Число слоев
        public int? PaintworkNumOfLayers { get; set; }

        // Группа грунтовки
        [Required]
        public int PrimerGroup { get; set; }

        // Можно красить
        [Required]
        public bool CanBePainted { get; set; }

        // Приоритет
        [Required]
        public int Priority { get; set; }
    }
}
