using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class AttachedDocUpdateRequest
    {
        public string Designation { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }

        public AttachedDocUpdateRequest()
        {
            Designation = null;
            Name = null;
            Note = null;
        }
    }
}
