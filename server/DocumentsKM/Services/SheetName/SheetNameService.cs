using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class SheetNameService : ISheetNameService
    {
        private readonly ISheetNameRepo _repository;

        public SheetNameService(ISheetNameRepo sheetNameRepo)
        {
            _repository = sheetNameRepo;
        }

        public IEnumerable<SheetName> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
