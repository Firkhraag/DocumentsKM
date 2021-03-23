using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IDefaultValuesRepo
    {
        // Получить значения по умолчанию по id пользователя
        DefaultValues GetByUserId(int userId);
        // Обновить значения по умолчанию
        void Update(DefaultValues defaultValues);
    }
}
