namespace DocumentsKM.Helpers
{
    public class AppSettings
    {
        public string JWTSecret { get; set; }
        public int TokenExpireTimeInDays { get; set; }
        public int DepartmentHeadPosId { get; set; }
        public int ChiefSpecialistPosId { get; set; }
        public int GroupLeaderPosId { get; set; }
        public int MainBuilderPosId { get; set; }
        public int ApprovalMinPosId { get; set; }
        public int ApprovalMaxPosId { get; set; }
    }
}
