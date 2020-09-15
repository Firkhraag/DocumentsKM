using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Model
{
    public class Mark
    {
        // Id_Марки
        [Key]
        public ulong Id { get; set; }

        // ОА_Подузел
        [Required]
        [ForeignKey("SubnodeId")]
        public Subnode Subnode { get; set; }

        // КодМарки
        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        // CREATE UNIQUE INDEX <name> ON (ОА_Подузел, КодМарки)

        // ДопКод
        [Required]
        [MaxLength(50)]
        public string AdditionalCode { get; set; }

        // НазвМарки
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Violating 3NF if next field is related to a department

        // Нач_отд 
        // FOREIGN_KEY из таблицы СписокРаботников
        [Required]
        public ulong DepartmentHeadId { get; set; }
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // Гл_спец
        // FOREIGN_KEY из таблицы СписокРаботников
        [Required]
        public ulong ChiefSpecialistId { get; set; }

        // Рук_гр 
        // FOREIGN_KEY из таблицы СписокРаботников
        [Required]
        public ulong GroupLeaderId { get; set; }

        // Гл_стр 
        // FOREIGN_KEY из таблицы СписокРаботников
        [Required]
        public ulong MainBulderId { get; set; }

        // Коэф_надежн
        [Required]
        public float SafetyСoefficient { get; set; }

        // Агрессивность
        [Required]
        public byte Aggressiveness { get; set; }

        // Т_эксплуат
        [Required]
        [MaxLength(10)]
        public string ExploitationT { get; set; }

        // Зона_эксплуат
        [Required]
        public byte ExploitationZone { get; set; }

        // Группа_газов
        [Required]
        public byte GasGroup { get; set; }

        // Материал
        [Required]
        public byte Material { get; set; }

        // Тип_ЛКМ
        [Required]
        [MaxLength(2)]
        public string LKMType { get; set; }

        // Вп_болты
        [Required]
        public byte VpBolts { get; set; }

        // ТекстЗдСм
        [Required]
        public string EstimateTaskText { get; set; }

        // ДопОбъемы
        [Required]
        public string AdditionalVolumes { get; set; }

        // !!!!!!!!!!!!!!
        // Isn't there a foreign key constraint with SpecificationRelease???
        // ТекВыпуск
        // [Required]
        // public byte CurrentRelease { get; set; }
        [Required]
        public ulong CurrentRelease { get; set; }

        //-----------------------------------------------------------

        // Отдел[1-7] из Отдел и Исп[1-7] из СписокРаботников
        // Код отдела и номер работника как FOREIGN KEY

        // Отдел1
        public ulong AgreedDepartment1 { get; set; }

        // Исп1
        public ulong AgreedWorker1 { get; set; }

        // Отдел2
        public ulong AgreedDepartment2 { get; set; }

        // Исп2
        public ulong AgreedWorker2 { get; set; }

        // Отдел3
        public ulong AgreedDepartment3 { get; set; }

        // Исп3
        public ulong AgreedWorker3 { get; set; }

        // Отдел4
        public ulong AgreedDepartment4 { get; set; }

        // Исп4
        public ulong AgreedWorker4 { get; set; }

        // Отдел5
        public ulong AgreedDepartment5 { get; set; }

        // Исп5
        public ulong AgreedWorker5 { get; set; }

        // Отдел6
        public ulong AgreedDepartment6 { get; set; }

        // Исп6
        public ulong AgreedWorker6 { get; set; }

        // Отдел7
        public ulong AgreedDepartment7 { get; set; }

        // Исп7
        public ulong AgreedWorker7 { get; set; }

        //-----------------------------------------------------------
        

        // Дата_ред
        [Required]
        [DataType(DataType.Date)]
        public DateTime EditedDate { get; set; }

        // Дата_выд
        [Required]
        [DataType(DataType.Date)]
        public DateTime IssuedDate { get; set; }
    }
}
