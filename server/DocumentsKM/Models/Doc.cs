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
        public int? ReleaseNum { get; set; }

        // Листов
        public int? NumOfPages { get; set; }

        // Прим
        [MaxLength(255)]
        public string Note { get; set; }

        public Doc()
        {
            Num = 1;
            Form = 1.0f;
            ReleaseNum = 0;
        }
    }
}