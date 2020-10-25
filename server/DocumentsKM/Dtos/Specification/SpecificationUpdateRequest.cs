namespace DocumentsKM.Dtos
{
    public class SpecificationUpdateRequest
    {
        public string Note { get; set; }
        public bool? IsCurrent { get; set; }

         public SpecificationUpdateRequest()
        {
            Note = null;
            IsCurrent = null;
        }
    }
}
