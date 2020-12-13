using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkGeneralDataRepo
    {
        // Получить общие данные марки по id
        MarkGeneralData GetById(string id);
        // Получить общие данные марки по id марки
        MarkGeneralData GetByMarkId(int markId);
        // Добавить общие данные марки
        MarkGeneralData Add(MarkGeneralData markGeneralData);
        // Обновить общие данные марки
        void Update(MarkGeneralData markGeneralDataIn);
        // Удалить общие данные марки
        void Delete(MarkGeneralData markGeneralDataIn);
    }
}
