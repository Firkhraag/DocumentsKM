namespace DocumentsKM.Dtos
{
    public class ConstructionElementUpdateRequest
    {
        public int? ProfileClassId { get; set; }

        public int? ProfileId { get; set; }

        public int? SteelId { get; set; }

        public float? Length { get; set; }

        public ConstructionElementUpdateRequest()
        {
            ProfileClassId = null;
            ProfileId = null;
            SteelId = null;
            Length = null;
        }
    }
}
