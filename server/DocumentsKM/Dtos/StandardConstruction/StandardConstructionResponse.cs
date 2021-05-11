using System;

namespace DocumentsKM.Dtos
{
    public class StandardConstructionResponse
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public Int16? Num { get; set; }
        public string Sheet { get; set; }
        public float Weight { get; set; }
    }
}
