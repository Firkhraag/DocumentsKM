using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class AttachedDocUpdateRequest
    {
        [MaxLength(40)]
        public string Designation { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Note { get; set; }

        public AttachedDocUpdateRequest()
        {
            Designation = null;
            Name = null;
            Note = null;
        }
    }
}
