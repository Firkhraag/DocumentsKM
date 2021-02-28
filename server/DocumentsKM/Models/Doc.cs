using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Документ основного комплекта марки
    public class Doc
    {
        [Key]
        public int Id { get; set; }

        // Марка
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        // Номер
        [Required]
        public int Num { get; set; }

        // Тип документа
        [Required]
        [ForeignKey("TypeId")]
        public virtual DocType Type { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Формат
        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Form { get; set; }

        // Разработал
        [ForeignKey("CreatorId")]
        public virtual Employee Creator { get; set; }
        public int? CreatorId { get; set; }

        // Проверил
        [ForeignKey("InspectorId")]
        public virtual Employee Inspector { get; set; }
        public int? InspectorId { get; set; }

        // Нормоконтролер
        [ForeignKey("NormContrId")]
        public virtual Employee NormContr { get; set; }
        public int? NormContrId { get; set; }

        // Выпуск
        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int ReleaseNum { get; set; }

        // Листов
        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int NumOfPages { get; set; }

        // Примечание
        [MaxLength(255)]
        public string Note { get; set; }
    }
}