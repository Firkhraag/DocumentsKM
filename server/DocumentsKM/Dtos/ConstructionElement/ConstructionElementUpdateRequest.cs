namespace DocumentsKM.Dtos
{
    public class ConstructionElementUpdateRequest
    {
        public int? ProfileClassId { get; set; }

        public string ProfileName { get; set; }

        public string Symbol { get; set; }

        public float? Weight { get; set; }

        public float? SurfaceArea { get; set; }

        public int? ProfileTypeId { get; set; }

        public int? SteelId { get; set; }

        public float? Length { get; set; }

        public int? Status { get; set; }

        public ConstructionElementUpdateRequest()
        {
            ProfileClassId = null;
            ProfileName = null;
            Symbol = null;
            Weight = null;
            SurfaceArea = null;
            ProfileTypeId = null;
            SteelId = null;
            Length = null;
            Status = null;
        }
    }
}
