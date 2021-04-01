using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Марка
    public class Mark
    {
        [Key]
        public Int32 Id { get; set; }

        // Подузел
        [Required]
        public Int32 SubnodeId { get; set; }

        // Обозначение
        [Required]
        [MaxLength(30)]
        public string Designation { get; set; }

        // Код
        [Required]
        [MaxLength(30)]
        public string Code { get; set; }

        // Название комплекса
        [MaxLength(255)]
        public string ComplexName { get; set; }

        // Название объекта
        [MaxLength(255)]
        public string ObjectName { get; set; }

        // Название
        [MaxLength(255)]
        public string Name { get; set; }

        // ГИП
        [MaxLength(255)]
        public string ChiefEngineerName { get; set; }

        // Отдел
        [Required]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        // Главный специалист
        [ForeignKey("ChiefSpecialistId")]
        public virtual Employee ChiefSpecialist { get; set; }
        public Int32? ChiefSpecialistId { get; set; }

        // Руководитель группы
        [ForeignKey("GroupLeaderId")]
        public virtual Employee GroupLeader { get; set; }
        public Int32? GroupLeaderId { get; set; }

        // Нормоконтролер
        [ForeignKey("NormContrId")]
        public virtual Employee NormContr { get; set; }
        public Int32? NormContrId { get; set; }

        // Дата редактирования
        public DateTime? EditedDate { get; set; }

        // Подп1
        public Int32? SignedId { get; set; }

        // Дата выдачи
        public DateTime? IssueDate { get; set; }

        // Количество томов
        public Int16? NumOfVolumes { get; set; }

        // Примечание
        public string Note { get; set; }

        // Категория пожарной опасности
        public Int16? FireHazardCategoryId { get; set; }
    }
}
