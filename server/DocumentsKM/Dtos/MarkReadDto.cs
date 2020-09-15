using DocumentsKM.Model;

namespace DocumentsKM.Dtos
{
    public class MarkReadDto
    {
        public ulong Id { get; set; }

        public Subnode Subnode { get; set; }

        public string Code { get; set; }

        public string AdditionalCode { get; set; }

        public string Name { get; set; }

        public Department Department { get; set; }

        public Employee ChiefSpecialist { get; set; }

        public Employee GroupLeader { get; set; }

        public Employee MainBulder { get; set; }

        public Employee AgreedWorker1 { get; set; }

        public Employee AgreedWorker2 { get; set; }

        public Employee AgreedWorker3 { get; set; }

        public Employee AgreedWorker4 { get; set; }

        public Employee AgreedWorker5 { get; set; }

        public Employee AgreedWorker6 { get; set; }

        public Employee AgreedWorker7 { get; set; }
    }
}
