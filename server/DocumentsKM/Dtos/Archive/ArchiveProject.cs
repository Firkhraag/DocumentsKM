using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class ArchiveProject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string NameShort { get; set; }
        public string NameCalculate => string.IsNullOrWhiteSpace(NameShort) ? Name : NameShort;

        public Project ToProject()
        {
            return new Project
            {
                
            };
        }
    }
}