using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.ArchDocT
{
    public class Mark
    {
        // For now we will consider NOT NULL constraint for every field

        // Марка
        [Key]
        public ulong Id { get; set; }

        // Подузел
        // FK т. Подузлы
        [Required]
        public ulong SubnodeId { get; set; }

        // КодМарки
        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        // КодМаркиДоп
        [Required]
        [MaxLength(40)]
        public string AdditionalCode { get; set; }

        // НазвМарки
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // НазвМаркиДоп
        [Required]
        [MaxLength(255)]
        public string AdditionalName { get; set; }

        // Отдел
        // FK т. Отделы
        [Required]
        public ulong DepartmentId { get; set; }

        // ГИПпоСпец
        // FOREIGN_KEY из таблицы СписокРаботников
        [Required]
        public ulong ChiefEngineerId { get; set; }

        // НачОтдела 
        // FOREIGN_KEY из таблицы СписокРаботников
        [Required]
        public ulong DepartmentHeadId { get; set; }

        // ГлСпец
        // FOREIGN_KEY из таблицы СписокРаботников
        [Required]
        public ulong ChiefSpecialistId { get; set; }

        // РукГруппы 
        // FOREIGN_KEY из таблицы СписокРаботников
        [Required]
        public ulong GroupLeaderId { get; set; }

        // КЧемуТип
        [Required]
        public uint TypeFor { get; set; }

        // КЧемуНомер
        [Required]
        public ulong NumberFor { get; set; }

        // Стадия
        [Required]
        [MaxLength(4)]
        public string Stage { get; set; }

        // Заказчик
        // FK ?
        [Required]
        public ulong Client { get; set; }

        // Наряд
        [Required]
        [MaxLength(20)]
        public string Contract { get; set; }

        // Контраг
        [Required]
        public bool IsContrag { get; set; }

        // СметаЕсть
        [Required]
        public bool HasEstimate { get; set; }

        // Задание
        [Required]
        [MaxLength(50)]
        public string Task { get; set; }

        // План
        [Required]
        public uint Plan { get; set; }

        // Пакет
        [Required]
        public ulong Package { get; set; }

        // СпецифЕсть
        [Required]
        public bool HasSpecification { get; set; }

        // Объект
        [Required]
        public ulong Object { get; set; }

        // ДатаМарка
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // id_mar_twf
        [Required]
        public ulong IdTwf { get; set; }

        // Прим
        [Required]
        [MaxLength(255)]
        public string Note { get; set; }

        // ВыпКМарке
        [Required]
        public ulong Extract { get; set; }
    }
}