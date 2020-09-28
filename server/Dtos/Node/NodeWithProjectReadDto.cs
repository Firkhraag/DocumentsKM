using System.Collections.Generic;

namespace DocumentsKM.Dtos
{
    public class NodeWithProjectReadDto
    {
        public ulong Id { get; set; }

        public ProjectSeriesReadDto Project { get; set; }

        public string Code { get; set; }

        // public List<SubnodeCodeReadDto> Subnodes { get; set; }
    }
}