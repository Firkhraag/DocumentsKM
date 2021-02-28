using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Марка
    public class Mark
    {
        [Key]
        public int Id { get; set; }

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
        public int? ChiefSpecialistId { get; set; }

        // Руководитель группы
        [ForeignKey("GroupLeaderId")]
        public virtual Employee GroupLeader { get; set; }
        public int? GroupLeaderId { get; set; }

        // Главный строитель
        [ForeignKey("MainBuilderId")]
        public virtual Employee MainBuilder { get; set; }

        // Дата редактирования
        public DateTime? EditedDate { get; set; }

        // Тут где-то ГИП, где-то начальник отдела
        public int? Signed1Id { get; set; }
        public int? Signed2Id { get; set; }

        // Дата выдачи
        public DateTime? IssuedDate { get; set; }

        public int? NumOfVolumes { get; set; }
        public float? SafetyCoeff { get; set; }
        public float? OperatingTemp { get; set; }
        public int? OperatingZone { get; set; }
        public int? GasGroup { get; set; }
        public int? Aggressiveness { get; set; }
        public int? Material { get; set; }
        public string PaintworkType { get; set; }
        public string Note { get; set; }
        public int? FireHazardCategoryId { get; set; }
        public int? HighTensileBolts { get; set; }
        public Boolean? P_transport { get; set; }
        public Boolean? P_site { get; set; }
        public Boolean? Xcnd { get; set; }
        public string Text_3d_estimate { get; set; }
        public string AddVolumes { get; set; }
        public float? VmpWeight { get; set; }
        public int? Impl_3d_estimate { get; set; }
    }
}
