using System;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkLinkedDocResponse
    {
        public Int16 Id { get; set; }
        public LinkedDoc LinkedDoc { get; set; }
        public string Note { get; set; }
    }
}
