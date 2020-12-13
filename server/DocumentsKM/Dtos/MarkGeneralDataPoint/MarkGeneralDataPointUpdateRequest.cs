namespace DocumentsKM.Dtos
{
    public class MarkGeneralDataPointUpdateRequest
    {
        public string Text { get; set; }
        public int? OrderNum { get; set; }

        public MarkGeneralDataPointUpdateRequest()
        {
            Text = null;
            OrderNum = null;
        }
    }
}
