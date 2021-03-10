using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Значения по умолчанию
    public class DefaultValues
    {
        [Key]
        public int Id { get; set; }

        // Отдел
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        // Разработчик
        [ForeignKey("CreatorId")]
        public virtual Employee Creator { get; set; }

        // Проверщик
        [ForeignKey("InspectorId")]
        public virtual Employee Inspector { get; set; }

        // Нормоконтролер
        [ForeignKey("NormContrId")]
        public virtual Employee NormContr { get; set; }
    }
}
