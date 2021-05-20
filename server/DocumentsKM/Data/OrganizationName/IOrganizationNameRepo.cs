using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IOrganizationNameRepo
    {
        // Получить название организации
        OrganizationName Get();
    }
}
