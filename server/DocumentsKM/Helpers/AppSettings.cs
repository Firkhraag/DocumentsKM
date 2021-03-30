namespace DocumentsKM.Helpers
{
    public class AppSettings
    {
        // JWT SECRET
        public string JWTSecret { get; set; }
        // Сколько дней живет JWT token
        public int TokenExpireTimeInDays { get; set; }

        // Начальник отдела
        public int DepartmentHeadPosId { get; set; }
        // И. о. начальника отдела
        public int ActingDepartmentHeadPosId { get; set; }
        // Заместитель начальника отдела
        public int DeputyDepartmentHeadPosId { get; set; }
        // И. о. заместителя начальника отдела
        public int ActingDeputyDepartmentHeadPosId { get; set; }

        // Главный специалист
        public int ChiefSpecialistPosId { get; set; }
        // И. о. главного специалиста
        public int ActingChiefSpecialistPosId { get; set; }

        // Заведующий группы
        public int GroupLeaderPosId { get; set; }
        // И. о. заведующего группы
        public int ActingGroupLeaderPosId { get; set; }

        // Главный строитель
        public int MainBuilderPosId { get; set; }

        // Согласования
        public int ApprovalMinPosId { get; set; }
        public int ApprovalMaxPosId { get; set; }
    }
}
