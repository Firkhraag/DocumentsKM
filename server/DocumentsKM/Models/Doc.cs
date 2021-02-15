using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Doc
    {
        // Id_листа
        [Key]
        public int Id { get; set; }

        // Id_марки
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        // Номер
        [Required]
        public int Num { get; set; }

        // Тип_док
        [Required]
        [ForeignKey("TypeId")]
        public virtual DocType Type { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Формат
        [Required]
        [Range(0.0f, 10000.0f, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Form { get; set; }

        // Разраб
        [ForeignKey("CreatorId")]
        public virtual Employee Creator { get; set; }

        // Пров
        [ForeignKey("InspectorId")]
        public virtual Employee Inspector { get; set; }

        // Н_контр
        [ForeignKey("NormContrId")]
        public virtual Employee NormContr { get; set; }

        // Выпуск
        [Required]
        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int ReleaseNum { get; set; }

        // Листов
        [Required]
        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int NumOfPages { get; set; }

        // Прим
        [MaxLength(255)]
        public string Note { get; set; }
    }
}