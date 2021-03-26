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

        public void Create(
            GeneralDataPoint generalDataPoint,
            int sectionId)
        {
            if (generalDataPoint == null)
                throw new ArgumentNullException(nameof(generalDataPoint));
            var foundSection = _generalDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                sectionId, generalDataPoint.Text);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            generalDataPoint.Section = foundSection;

            var currentPoints = _repository.GetAllBySectionId(sectionId);
            if (currentPoints.Count() == 0)
                generalDataPoint.OrderNum = 1;
            else
                generalDataPoint.OrderNum = (Int16)(currentPoints.Max(v => v.OrderNum) + 1);
            
            _repository.Add(generalDataPoint);
        }

        public void Update(
            int id, int sectionId,
            GeneralDataPointUpdateRequest generalDataPoint)
        {
            if (generalDataPoint == null)
                throw new ArgumentNullException(nameof(generalDataPoint));
            var foundGeneralDataPoint = _repository.GetById(id);
            if (foundGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundGeneralDataPoint));
            var foundSection = _generalDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));

            if (generalDataPoint.Text != null)
            {
                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
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
                short num = 1;
                foreach (var p in _repository.GetAllBySectionId(sectionId))
                {
                    if (p.Id == id)
                        continue;
                    if (num == generalDataPoint.OrderNum)
                    {
                        num = (Int16)(num + 1);
                        p.OrderNum = num;
                        _repository.Update(p);
                        num = (Int16)(num + 1);
                        continue;
                    }
                    p.OrderNum = num;
                    _repository.Update(p);
                    num = (Int16)(num + 1);
                }
            }
            _repository.Update(foundGeneralDataPoint);
        }

        public void Delete(int id, int sectionId)
        {
            var foundGeneralDataPoint = _repository.GetById(id);
            if (foundGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundGeneralDataPoint));
            var foundSection = _generalDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));
            foreach (var p in _repository.GetAllBySectionId(sectionId))
            {
                if (p.OrderNum > foundGeneralDataPoint.OrderNum)
                {
                    p.OrderNum = (Int16)(p.OrderNum - 1);
                    _repository.Update(p);
                }
            }
            _repository.Delete(foundGeneralDataPoint);
        }
    }
}
