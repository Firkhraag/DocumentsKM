using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using System.Linq;

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

        // public void Copy(
        //     int userId, GeneralDataSectionCopyRequest generalDataSectionRequest)
        // {
        //     if (generalDataSectionRequest == null)
        //         throw new ArgumentNullException(nameof(generalDataSectionRequest));
        //     var foundUser = _userRepo.GetById(userId);
        //     if (foundUser == null)
        //         throw new ArgumentNullException(nameof(foundUser));
        //     var foundMarkGeneralDataSection = _markGeneralDataSectionRepo.GetById(
        //         generalDataSectionRequest.Id, true);
        //     if (foundUser == null)
        //         throw new ArgumentNullException(nameof(foundUser));

        //     var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
        //         userId, generalDataSectionRequest.Name);
        //     if (uniqueConstraintViolationCheck != null)
        //         throw new ConflictException(nameof(uniqueConstraintViolationCheck));

        //     short orderNum;
        //     var currentSection = _repository.GetAllByUserId(userId);
        //     if (currentSection.Count() == 0)
        //         orderNum = 1;
        //     else
        //         orderNum = (Int16)(currentSection.Max(v => v.OrderNum) + 1);

        //     var generalDataSection = new GeneralDataSection
        //     {
        //         User = foundUser,
        //         Name = generalDataSectionRequest.Name,
        //         OrderNum = orderNum,
        //         GeneralDataPoints = new List<GeneralDataPoint>(){},
        //     };

        //     _repository.Add(generalDataSection);
        //     foreach (var p in foundMarkGeneralDataSection.MarkGeneralDataPoints)
        //     {
        //         _generalDataPointRepo.Add(new GeneralDataPoint
        //         {
        //             Section = generalDataSection,
        //             Text = p.Text,
        //             OrderNum = p.OrderNum,
        //         });
        //     }
        // }
    }
}
