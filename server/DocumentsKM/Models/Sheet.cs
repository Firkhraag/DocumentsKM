using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Sheet
    {
        // Id_листа
        [Key]
        public int Id { get; set; }

        // Id_марки
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }
        public int MarkId { get; set; }

        // Номер
        [Required]
        public int Number { get; set; }

        // Тип_док
        [Required]
        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }
        public byte DocumentTypeId { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Формат
        [Required]
        public float Format { get; set; }

        // Разраб
        [Required]
        [ForeignKey("DeveloperId")]
        public virtual Employee Developer { get; set; }

        // Пров
        [ForeignKey("InspectorId")]
        public virtual Employee Inspector { get; set; }

        // Н_контр
        [ForeignKey("NormControllerId")]
        public virtual Employee NormController { get; set; }

        // Выпуск
        [Required]
        public byte Release { get; set; }

        // Листов
        [Required]
        public byte NumberOfPages { get; set; }

        // Прим
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