using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class GeneralDataSectionService : IGeneralDataSectionService
    {
        private readonly IGeneralDataSectionRepo _repository;
        private readonly IGeneralDataPointRepo _generalDataPointRepo;
        private readonly IMarkGeneralDataSectionRepo _markGeneralDataSectionRepo;
        private readonly IUserRepo _userRepo;

        public GeneralDataSectionService(
            IGeneralDataSectionRepo generalDataSectionRepo,
            IGeneralDataPointRepo generalDataPointRepo,
            IMarkGeneralDataSectionRepo markGeneralDataSectionRepo,
            IUserRepo userRepo)
        {
            _repository = generalDataSectionRepo;
            _generalDataPointRepo = generalDataPointRepo;
            _markGeneralDataSectionRepo = markGeneralDataSectionRepo;
            _userRepo = userRepo;
        }

        public IEnumerable<GeneralDataSection> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
