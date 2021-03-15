namespace Gipromez.JobLog.External.Archive.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string NameShort { get; set; }
        public string NameCalculate => string.IsNullOrWhiteSpace(NameShort) ? Name : NameShort;
    }
}