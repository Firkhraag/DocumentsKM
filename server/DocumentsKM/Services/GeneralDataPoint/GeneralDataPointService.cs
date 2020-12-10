using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class GeneralDataPointService : IGeneralDataPointService
    {
        private IGeneralDataPointRepo _repository;
        private readonly IGeneralDataSectionRepo _generalDataSectionRepo;
        private readonly IUserRepo _userRepo;

        public GeneralDataPointService(IGeneralDataPointRepo generalDataPointRepo,
            IGeneralDataSectionRepo generalDataSectionRepo,
            IUserRepo userRepo)
        {
            _repository = generalDataPointRepo;
            _generalDataSectionRepo = generalDataSectionRepo;
            _userRepo = userRepo;
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

            var uniqueConstraintViolationCheck = _repository.GetByUserAndSectionIdAndText(
                userId, sectionId, generalDataPoint.Text);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            generalDataPoint.Section = foundSection;
            generalDataPoint.User = foundUser;

            int maxNum = 0;
            foreach (var p in _repository.GetAllByUserAndSectionId(userId, sectionId))
            {
                if (p.OrderNum > maxNum)
                    maxNum = p.OrderNum;
            }
            generalDataPoint.OrderNum = maxNum + 1;
            
            _repository.Add(generalDataPoint);
        }

        public void Update(
            int id,
            GeneralDataPointUpdateRequest generalDataPoint)
        {
            if (generalDataPoint == null)
                throw new ArgumentNullException(nameof(generalDataPoint));
            var foundGeneralDataPoint = _repository.GetById(id);
            if (foundGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundGeneralDataPoint));

            if (generalDataPoint.Text != null)
            {
                var uniqueConstraintViolationCheck = _repository.GetByUserAndSectionIdAndText(
                    foundGeneralDataPoint.User.Id,
                    foundGeneralDataPoint.Section.Id,
                    generalDataPoint.Text);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
                foundGeneralDataPoint.Text = generalDataPoint.Text;
            }
            if (generalDataPoint.OrderNum != null)
            {
                var orderNum = generalDataPoint.OrderNum.GetValueOrDefault();
                var uniqueConstraintViolationCheck = _repository.GetByUserAndSectionIdAndOrderNum(
                    foundGeneralDataPoint.User.Id, foundGeneralDataPoint.Section.Id, orderNum);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
                foundGeneralDataPoint.OrderNum = orderNum;
            }

            _repository.Update(foundGeneralDataPoint);
        }

        public void Delete(int id)
        {
            var foundGeneralDataPoint = _repository.GetById(id);
            if (foundGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundGeneralDataPoint));
            _repository.Delete(foundGeneralDataPoint);
        }
    }
}
