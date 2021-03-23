using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Документ основного комплекта марки
    public class Doc
    {
        [Key]
        public Int32 Id { get; set; }

        // Марка
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        // Номер
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Num { get; set; }

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
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Form { get; set; }

        // Разработал
        [Required]
        [ForeignKey("CreatorId")]
        public virtual Employee Creator { get; set; }
        public Int32 CreatorId { get; set; }

        // Проверил
        [ForeignKey("InspectorId")]
        public virtual Employee Inspector { get; set; }
        public Int32? InspectorId { get; set; }

        // Нормоконтролер
        [ForeignKey("NormContrId")]
        public virtual Employee NormContr { get; set; }
        public Int32? NormContrId { get; set; }

        // Выпуск
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 ReleaseNum { get; set; }

        // Листов
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 NumOfPages { get; set; }

        // Примечание
        [MaxLength(255)]
        public string Note { get; set; }
    }
}