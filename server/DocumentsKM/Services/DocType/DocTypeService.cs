using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class DocTypeService : IDocTypeService
    {
        // Id листа основного комплекта из справочника типов документов
        private readonly int _sheetDocTypeId = 1;

        private IDocTypeRepo _repository;

        public DocTypeService(IDocTypeRepo docTypeRepoo)
        {
            _repository = docTypeRepoo;
        }

        public IEnumerable<DocType> GetAllAttached()
        {
            return _repository.GetAllExceptId(_sheetDocTypeId);
        }
    }
}
