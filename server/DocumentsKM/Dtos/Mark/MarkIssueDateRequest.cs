using System;

namespace DocumentsKM.Dtos
{
    public class MarkIssueDateRequest
    {
        public DateTime? IssueDate { get; set; }

        public MarkIssueDateRequest()
        {
            IssueDate = null;
        }
    }
}
