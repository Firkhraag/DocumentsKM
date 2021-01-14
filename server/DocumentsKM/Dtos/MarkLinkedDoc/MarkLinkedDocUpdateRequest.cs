using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkLinkedDocUpdateRequest
    {
        public int? LinkedDocId { get; set; }

        [MaxLength(50)]
        public string Note { get; set; }

        public MarkLinkedDocUpdateRequest()
        {
            LinkedDocId = null;
            Note = null;
        }
    }
}
