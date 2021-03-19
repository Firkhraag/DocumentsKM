using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class ArchiveNode
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }

        public Node ToNode()
        {
            return new Node
            {
                
            };
        }

        public Subnode ToSubnode()
        {
            return new Subnode
            {
                
            };
        }
    }
}