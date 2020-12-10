namespace DocumentsKM.Dtos
{
    public class GeneralDataPointUpdateRequest
    {
        public string Text { get; set; }
        public int? OrderNum { get; set; }

        public GeneralDataPointUpdateRequest()
        {
            Text = null;
            OrderNum = null;
        }
    }
}
