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

        public IEnumerable<GeneralDataPoint> GetAllByUserAndSectionId(
            int userId, int sectionId)
        {
            return _repository.GetAllByUserAndSectionId(userId, sectionId);
        }

        public void Create(
            GeneralDataPoint generalDataPoint,
            int userId,
            int sectionId)
        {
            if (generalDataPoint == null)
                throw new ArgumentNullException(nameof(generalDataPoint));
            var foundSection = _generalDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));
            var foundUser = _userRepo.GetById(userId);
            if (foundUser == null)
                throw new ArgumentNullException(nameof(foundUser));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                userId, sectionId, generalDataPoint.Text);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            generalDataPoint.Section = foundSection;
            generalDataPoint.User = foundUser;

            var currentPoints = _repository.GetAllByUserAndSectionId(userId, sectionId);
            if (currentPoints.Count() == 0)
                generalDataPoint.OrderNum = 1;
            else
                generalDataPoint.OrderNum = currentPoints.Max(v => v.OrderNum) + 1;
            
            _repository.Add(generalDataPoint);
        }

        public void Update(
            int id, int userId, int sectionId,
            GeneralDataPointUpdateRequest generalDataPoint)
        {
            if (generalDataPoint == null)
                throw new ArgumentNullException(nameof(generalDataPoint));
            var foundGeneralDataPoint = _repository.GetById(id);
            if (foundGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundGeneralDataPoint));
            var foundUser = _userRepo.GetById(userId);
            if (foundUser == null)
                throw new ArgumentNullException(nameof(foundUser));
            var foundSection = _generalDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));

            if (generalDataPoint.Text != null)
            {
                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                    foundGeneralDataPoint.User.Id,
                    foundGeneralDataPoint.Section.Id,
                    generalDataPoint.Text);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
                foundGeneralDataPoint.Text = generalDataPoint.Text;
            }
            if (generalDataPoint.OrderNum != null && generalDataPoint.OrderNum != foundGeneralDataPoint.OrderNum)
            {
                var orderNum = generalDataPoint.OrderNum.GetValueOrDefault();
                foundGeneralDataPoint.OrderNum = orderNum;
                var num = 1;
                foreach (var p in _repository.GetAllByUserAndSectionId(userId, sectionId))
                {
                    if (p.Id == id)
                        continue;
                    if (num == generalDataPoint.OrderNum)
                    {
                        num = num + 1;
                        p.OrderNum = num;
                        _repository.Update(p);
                        num = num + 1;
                        continue;
                    }
                    p.OrderNum = num;
                    _repository.Update(p);
                    num = num + 1;
                }
            }
            _repository.Update(foundGeneralDataPoint);
        }

        public void Delete(int id, int userId, int sectionId)
        {
            var foundGeneralDataPoint = _repository.GetById(id);
            if (foundGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundGeneralDataPoint));
            var foundUser = _userRepo.GetById(userId);
            if (foundUser == null)
                throw new ArgumentNullException(nameof(foundUser));
            var foundSection = _generalDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));
            foreach (var p in _repository.GetAllByUserAndSectionId(userId, sectionId))
            {
                if (p.OrderNum > foundGeneralDataPoint.OrderNum)
                {
                    p.OrderNum = p.OrderNum - 1;
                    _repository.Update(p);
                }
            }
            _repository.Delete(foundGeneralDataPoint);
        }
    }
}
