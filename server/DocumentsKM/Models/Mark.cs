using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Mark
    {
        // Id_Марки
        [Key]
        public int Id { get; set; }

        // ОА_Подузел
        [Required]
        [ForeignKey("SubnodeId")]
        public virtual Subnode Subnode { get; set; }
        public int SubnodeId { get; set; }

        // КодМарки
        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        // ДопКод
        [MaxLength(50)]
        public string AdditionalCode { get; set; }

        // НазвМарки
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Код_отд
        [Required]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        // Гл_спец
        [ForeignKey("ChiefSpecialistId")]
        public virtual Employee ChiefSpecialist { get; set; }

        // Рук_гр 
        [ForeignKey("GroupLeaderId")]
        public virtual Employee GroupLeader { get; set; }

        // Гл_стр 
        [Required]
        [ForeignKey("MainBuilderId")]
        public virtual Employee MainBuilder { get; set; }

        // Дата_ред
        [Required]
        [DataType(DataType.Date)]
        public DateTime Edited { get; set; }
    }
}
