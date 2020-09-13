using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.ArchDocT
{
    public class Node
    {
        // For now we will consider NOT NULL constraint for every field

        // Узел
        [Key]
        public ulong Id { get; set; }

        // Проект
        // FK т. Проекты
        [Required]
        public ulong ProjectId { get; set; }

        // КодУзла
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        // НазвУзла
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // НазвУзлаДоп
        [Required]
        [MaxLength(255)]
        public string AdditionalName { get; set; }

        // ГИП
        // FK т. СписокРаботников
        [Required]
        public ulong ChiefEngineerId { get; set; }

        // АктивУзел
        [Required]
        [MaxLength(30)]
        public string ActiveNode { get; set; }

        // ДатаУзел
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // id_uz_twf
        [Required]
        public ulong IdTwf { get; set; }
    }
}