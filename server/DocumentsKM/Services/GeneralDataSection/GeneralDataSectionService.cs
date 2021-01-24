using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class GeneralDataSectionService : IGeneralDataSectionService
    {
        private IGeneralDataSectionRepo _repository;

        public GeneralDataSectionService(
            IGeneralDataSectionRepo generalDataSectionRepo)
        {
            _repository = generalDataSectionRepo;
        }

        public IEnumerable<GeneralDataSection> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
