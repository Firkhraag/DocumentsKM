using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.ProjectKM
{
    public class GeneralDirectivePoint
    {
        // For now we will consider NOT NULL constraint for every field

        // Поз_пункта
        [Key]
        public ulong Position { get; set; }

        // Id_марки
        // FOREIGN KEY т. Марки
        [Required]
        public ulong MarkId { get; set; }

        // пункт_о
        // FOREIGN KEY т. Спр_пункты
        [Required]
        public byte Point { get; set; }

        // CREATE UNIQUE INDEX <name> ON (Id_марки, пункт_о)

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // текст_о
        // IsOriginal ? _ : FOREIGN KEY т. Спр_пункты
        // [Required]
        // public string Text { get; set; }

        // Only one is not null ->

        // текст_о
        public string OriginalText { get; set; }

        // текст_о
        // FOREIGN KEY т. Спр_пункты
        public ulong StandardTextId { get; set; }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // включать
        [Required]
        public bool IsIncluded { get; set; }

        // оригинал
        // [Required]
        // public bool IsOriginal { get; set; }

        // неразрывать_о
        [Required]
        public bool shouldNotBreak { get; set; }

        // Тип_пункта
        [Required]
        public byte Type { get; set; }

        public GeneralDirectivePoint()
        {
            Type = 0;
        }
    }
}