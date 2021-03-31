using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using DocumentsKM.Helpers;
using Microsoft.Extensions.Options;

namespace DocumentsKM.Services
{
    public class DocTypeService : IDocTypeService
    {
        private readonly IDocTypeRepo _repository;
        private readonly AppSettings _appSettings;

        public DocTypeService(
            IDocTypeRepo docTypeRepo,
            IOptions<AppSettings> appSettings)
        {
            _repository = docTypeRepo;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<DocType> GetAllAttached()
        {
            return _repository.GetAllExceptId(_appSettings.SheetDocTypeId);
        }
    }
}
