using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Вариант антикоррозионной защиты
    public class CorrProtVariant
    {
        [Key]
        public int Id { get; set; }

        // Зона эксплуатации
        [Required]
        public int ExploitationZone { get; set; }

        // Группа газов
        [Required]
        public int GasGroup { get; set; }

        // Агрессивность среды
        [Required]
        public int EnvAggressiveness { get; set; }

        // Материал
        [Required]
        public int Material { get; set; }

        // Тип лакокрасочного материала
        [Required]
        [MaxLength(2)]
        public string PaintworkType { get; set; }

        // Группа покраски
        [Required]
        public int PaintworkGroup { get; set; }

        // Стойкость
        [Required]
        [MaxLength(2)]
        public string PaintworkDurability { get; set; }

        // Число слоев
        [Required]
        public int PaintworkNumOfLayers { get; set; }

        // Толщина покраски
        [Required]
        public int PaintworkPrimerThickness { get; set; }

        // Количество слоев грунтовки
        [Required]
        public int PrimerNumOfLayers { get; set; }

        // Степень очистки
        [Required]
        public virtual CorrProtCleaningDegree CleaningDegree { get; set; }
    }
}
