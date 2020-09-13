using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.ArchDocT
{
    public class Subnode
    {
        // For now we will consider NOT NULL constraint for every field

        // Подузел
        [Key]
        public ulong Id { get; set; }

        // Узел
        // FK т. Узлы
        [Required]
        public ulong NodeId { get; set; }

        // КодПодуз
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        // НазвПодузла
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // НазвПодузлаДоп
        [Required]
        [MaxLength(255)]
        public string AdditionalName { get; set; }

        // ДатаПодуз
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // id_puz_twf
        [Required]
        public ulong IdTwf { get; set; }
    }
}