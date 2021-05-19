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

        public IEnumerable<GeneralDataSection> GetAllByUserId(int userId)
        {
            return _repository.GetAllByUserId(userId);
        }

        public void Create(
            GeneralDataSection generalDataSection,
            int userId)
        {
            if (generalDataSection == null)
                throw new ArgumentNullException(nameof(generalDataSection));
            var foundUser = _userRepo.GetById(userId);
            if (foundUser == null)
                throw new ArgumentNullException(nameof(foundUser));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                userId, generalDataSection.Name);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(nameof(uniqueConstraintViolationCheck));

            generalDataSection.User = foundUser;

            var currentSection = _repository.GetAllByUserId(userId);
            if (currentSection.Count() == 0)
                generalDataSection.OrderNum = 1;
            else
                generalDataSection.OrderNum = (Int16)(currentSection.Max(v => v.OrderNum) + 1);

            _repository.Add(generalDataSection);
        }

        public void Update(
            int id, int userId,
            GeneralDataSectionUpdateRequest generalDataSection)
        {
            if (generalDataSection == null)
                throw new ArgumentNullException(nameof(generalDataSection));
            var foundGeneralDataSection = _repository.GetById(id);
            if (foundGeneralDataSection == null)
                throw new ArgumentNullException(nameof(foundGeneralDataSection));
            var foundUser = _userRepo.GetById(userId);
            if (foundUser == null)
                throw new ArgumentNullException(nameof(foundUser));

            if (generalDataSection.Name != null)
            {
                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                    foundGeneralDataSection.User.Id,
                    generalDataSection.Name);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(nameof(uniqueConstraintViolationCheck));
                foundGeneralDataSection.Name = generalDataSection.Name;
            }
            if (generalDataSection.OrderNum != null)
            {
                var orderNum = generalDataSection.OrderNum.GetValueOrDefault();
                foundGeneralDataSection.OrderNum = orderNum;
                short num = 1;
                foreach (var s in _repository.GetAllByUserId(userId))
                {
                    if (s.Id == id)
                        continue;
                    if (num == orderNum)
                    {
                        num = (Int16)(num + 1);
                        s.OrderNum = num;
                        _repository.Update(s);
                        num = (Int16)(num + 1);
                        continue;
                    }
                    s.OrderNum = num;
                    _repository.Update(s);
                    num = (Int16)(num + 1);
                }
            }

            _repository.Update(foundGeneralDataSection);
        }

        public void Delete(int id, int userId)
        {
            var foundGeneralDataSection = _repository.GetById(id, true);
            if (foundGeneralDataSection == null)
                throw new ArgumentNullException(nameof(foundGeneralDataSection));
            var foundUser = _userRepo.GetById(userId);
            if (foundUser == null)
                throw new ArgumentNullException(nameof(foundUser));
            foreach (var p in _repository.GetAllByUserId(userId))
            {
                if (p.OrderNum > foundGeneralDataSection.OrderNum)
                {
                    p.OrderNum = (Int16)(p.OrderNum - 1);
                    _repository.Update(p);
                }
            }
            _repository.Delete(foundGeneralDataSection);
        }

        public void Copy(
            int userId, GeneralDataSectionCopyRequest generalDataSectionRequest)
        {
            if (generalDataSectionRequest == null)
                throw new ArgumentNullException(nameof(generalDataSectionRequest));
            var foundUser = _userRepo.GetById(userId);
            if (foundUser == null)
                throw new ArgumentNullException(nameof(foundUser));
            var foundMarkGeneralDataSection = _markGeneralDataSectionRepo.GetById(
                generalDataSectionRequest.Id, true);
            if (foundUser == null)
                throw new ArgumentNullException(nameof(foundUser));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                userId, generalDataSectionRequest.Name);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(nameof(uniqueConstraintViolationCheck));

            short orderNum;
            var currentSection = _repository.GetAllByUserId(userId);
            if (currentSection.Count() == 0)
                orderNum = 1;
            else
                orderNum = (Int16)(currentSection.Max(v => v.OrderNum) + 1);

            var generalDataSection = new GeneralDataSection
            {
                User = foundUser,
                Name = generalDataSectionRequest.Name,
                OrderNum = orderNum,
                GeneralDataPoints = new List<GeneralDataPoint>(){},
            };

            _repository.Add(generalDataSection);
            foreach (var p in foundMarkGeneralDataSection.MarkGeneralDataPoints)
            {
                _generalDataPointRepo.Add(new GeneralDataPoint
                {
                    Section = generalDataSection,
                    Text = p.Text,
                    OrderNum = p.OrderNum,
                });
            }

        }
    }
}
