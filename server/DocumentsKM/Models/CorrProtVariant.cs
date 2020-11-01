using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class CorrProtVariant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ExploitationZone { get; set; }

        [Required]
        public int GasGroup { get; set; }

        [Required]
        public int EnvAggressiveness { get; set; }

        [Required]
        public int Material { get; set; }

        [Required]
        [MaxLength(2)]
        public string PaintworkType { get; set; }

        [Required]
        public int PaintworkGroup { get; set; }

        [Required]
        [MaxLength(2)]
        public string PaintworkDurability { get; set; }

        [Required]
        public int PaintworkNumOfLayers { get; set; }

        [Required]
        public int PaintworkPrimerThickness { get; set; }

        [Required]
        public int PrimerNumOfLayers { get; set; }

        [Required]
        public virtual CorrProtCleaningDegree CleaningDegree { get; set; }


        // [Key]
        // public Int16 Id { get; set; }

        // [Required]
        // public Int16 ExploitationZone { get; set; }

        // [Required]
        // public Int16 GasGroup { get; set; }

        // [Required]
        // public Int16 EnvAggressiveness { get; set; }

        // [Required]
        // public Int16 Material { get; set; }

        // [Required]
        // [MaxLength(2)]
        // public string PaintworkType { get; set; }

        // [Required]
        // public Int16 PaintworkGroup { get; set; }

        // [Required]
        // [MaxLength(2)]
        // public string PaintworkDurability { get; set; }

        // [Required]
        // public Int16 PaintworkNumOfLayers { get; set; }

        // [Required]
        // public Int16 PaintworkPrimerThickness { get; set; }

        // [Required]
        // public Int16 PrimerNumOfLayers { get; set; }

        // [Required]
        // public virtual CorrProtCleaningDegree CleaningDegree { get; set; }
    }
}
