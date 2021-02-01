using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class ConstructionElementResponse
    {
        public int Id { get; set; }
        public ProfileClass ProfileClass { get; set; }
        public string ProfileName { get; set; }
        public string Symbol { get; set; }
        public float Weight { get; set; }
        public float SurfaceArea { get; set; }
        public ProfileType ProfileType { get; set; }
        public Steel Steel { get; set; }
        public float Length { get; set; }
        public int Status { get; set; }
    }
}
