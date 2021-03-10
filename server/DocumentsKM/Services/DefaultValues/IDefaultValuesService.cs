using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IDefaultValuesService
    {
        // Получить значения по умолчанию по id пользователя
        DefaultValues GetByUserId(int userId);
        // Изменить значения по умолчанию у пользователя
        void Update(int userId, DefaultValuesUpdateRequest defaultValuesUpdateRequest);
    }
}
