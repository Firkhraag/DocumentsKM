using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class Sheet
    {
        // For now we will consider NOT NULL constraint for every field

        // Id_листа
        [Key]
        public ulong Id { get; set; }

        // Id_марки
        // FOREIGN KEY т. Марки
        [Required]
        public ulong MarkId { get; set; }

        // Номер
        [Required]
        public uint Number { get; set; }

        // Тип_док
        // FOREIGN KEY т. Типы_док
        [Required]
        public byte DocumentType { get; set; }

        // CREATE UNIQUE INDEX <name> ON (Id_марки, Номер, Тип_док)

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Формат
        [Required]
        public float Format { get; set; }

        // Разраб
        // FOREIGN KEY т. СписокРаботников
        [Required]
        public ulong DevelopedPersonId { get; set; }

        // Пров
        // FOREIGN KEY т. СписокРаботников
        [Required]
        public ulong CheckedPersonId { get; set; }

        // Н_контр
        // FOREIGN KEY т. СписокРаботников
        [Required]
        public ulong NormControlledPersonId { get; set; }

        // Выпуск
        [Required]
        public byte Release { get; set; }

        // Листов
        [Required]
        public byte NumberOfPages { get; set; }

        // Прим
        [Required]
        [MaxLength(50)]
        public string Note { get; set; }

        public Sheet()
        {
            Number = 1;
            Format = 1.0f;
            Release = 0;
        }

    }
}