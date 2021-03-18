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
        [ForeignKey("SubnodeId")]
        public virtual Subnode Subnode { get; set; }

        // Код
        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

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

        // Главный строитель
        [ForeignKey("MainBuilderId")]
        public virtual Employee MainBuilder { get; set; }

        // Дата редактирования
        public DateTime? EditedDate { get; set; }

        // Тут где-то ГИП, где-то начальник отдела
        public Int32? Signed1Id { get; set; }
        public Int32? Signed2Id { get; set; }

        // Дата выдачи
        public DateTime? IssueDate { get; set; }

        public Int16? NumOfVolumes { get; set; }
        public string PaintworkType { get; set; }
        public string Note { get; set; }
        public Int16? FireHazardCategoryId { get; set; }
        public Boolean? PTransport { get; set; }
        public Boolean? PSite { get; set; }
    }
}
