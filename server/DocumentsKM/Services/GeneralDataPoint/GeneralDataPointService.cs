using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using System.Linq;

namespace DocumentsKM.Services
{
    public class GeneralDataPointService : IGeneralDataPointService
    {
        private readonly IGeneralDataPointRepo _repository;
        private readonly IUserRepo _userRepo;
        private readonly IGeneralDataSectionRepo _generalDataSectionRepo;

        public GeneralDataPointService(
            IGeneralDataPointRepo generalDataPointRepo,
            IUserRepo userRepo,
            IGeneralDataSectionRepo generalDataSectionRepo)
        {
            _repository = generalDataPointRepo;
            _userRepo = userRepo;
            _generalDataSectionRepo = generalDataSectionRepo;
        }

        public IEnumerable<GeneralDataPoint> GetAllBySectionId(
            int sectionId)
        {
            return _repository.GetAllBySectionId(sectionId);
        }

        public IEnumerable<GeneralDataPoint> GetAllBySectionName(string sectionName)
        {
            return _repository.GetAllBySectionName(sectionName);
        }
    }
}
