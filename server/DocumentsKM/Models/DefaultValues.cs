using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Значения по умолчанию
    public class DefaultValues
    {
        // Пользователь
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Key]
        public int UserId { get; set; }

        // Отдел
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public int? DepartmentId { get; set; }

        // Разработчик
        [ForeignKey("CreatorId")]
        public virtual Employee Creator { get; set; }
        public int? CreatorId { get; set; }

        // Проверщик
        [ForeignKey("InspectorId")]
        public virtual Employee Inspector { get; set; }
        public int? InspectorId { get; set; }

        // Нормоконтролер
        [ForeignKey("NormContrId")]
        public virtual Employee NormContr { get; set; }
        public int? NormContrId { get; set; }
    }
}
