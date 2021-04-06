using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IPrimerRepo
    {
        // Получить грунтовку по группе
        Primer GetByGroup(int group);
    }
}
