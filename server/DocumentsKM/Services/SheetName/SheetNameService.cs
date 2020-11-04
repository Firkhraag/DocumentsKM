using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class SheetNameService : ISheetNameService
    {
        private ISheetNameRepo _repository;

        public SheetNameService(ISheetNameRepo sheetNameRepoo)
        {
            _repository = sheetNameRepoo;
        }

        public IEnumerable<SheetName> GetAll()
        {
            return _repository.GetAll();
        }
    }
}