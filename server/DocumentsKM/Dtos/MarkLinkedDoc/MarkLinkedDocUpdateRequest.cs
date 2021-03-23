using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkLinkedDocUpdateRequest
    {
        public Int16? LinkedDocId { get; set; }

        [MaxLength(50)]
        public string Note { get; set; }

        public MarkLinkedDocUpdateRequest()
        {
            LinkedDocId = null;
            Note = null;
        }
    }
}
