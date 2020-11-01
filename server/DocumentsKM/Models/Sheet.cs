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
        public int Num { get; set; }

        // Тип_док
        [Required]
        [ForeignKey("DocTypeId")]
        public virtual DocType DocType { get; set; }
        public int DocTypeId { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Формат
        [Required]
        public float Form { get; set; }

        // Разраб
        [Required]
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
        public int ReleaseNum { get; set; }

        // Листов
        [Required]
        public int NumOfSheets { get; set; }

        // Прим
        [MaxLength(50)]
        public string Note { get; set; }

        // // Id_листа
        // [Key]
        // public int Id { get; set; }

        // // Id_марки
        // [Required]
        // [ForeignKey("MarkId")]
        // public virtual Mark Mark { get; set; }
        // public int MarkId { get; set; }

        // // Номер
        // [Required]
        // public Int16 Num { get; set; }

        // // Тип_док
        // [Required]
        // [ForeignKey("DocTypeId")]
        // public virtual DocType DocType { get; set; }
        // public Int16 DocTypeId { get; set; }

        // // Название
        // [Required]
        // [MaxLength(255)]
        // public string Name { get; set; }

        // // Формат
        // [Required]
        // public float Form { get; set; }

        // // Разраб
        // [Required]
        // [ForeignKey("CreatorId")]
        // public virtual Employee Creator { get; set; }

        // // Пров
        // [ForeignKey("InspectorId")]
        // public virtual Employee Inspector { get; set; }

        // // Н_контр
        // [ForeignKey("NormContrId")]
        // public virtual Employee NormContr { get; set; }

        // // Выпуск
        // [Required]
        // public Int16 ReleaseNum { get; set; }

        // // Листов
        // [Required]
        // public Int16 NumOfSheets { get; set; }

        // // Прим
        // [MaxLength(50)]
        // public string Note { get; set; }

        public Sheet()
        {
            Num = 1;
            Form = 1.0f;
            ReleaseNum = 0;
        }
    }
}