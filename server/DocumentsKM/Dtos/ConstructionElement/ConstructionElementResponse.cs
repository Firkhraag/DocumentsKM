using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class ConstructionElementResponse
    {
        public int Id { get; set; }
        public Profile Profile { get; set; }
        public Steel Steel { get; set; }
        public float Length { get; set; }
    }
}
