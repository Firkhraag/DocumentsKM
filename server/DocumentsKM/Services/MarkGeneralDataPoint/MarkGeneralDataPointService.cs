using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class MarkGeneralDataPointService : IMarkGeneralDataPointService
    {
        private IMarkGeneralDataPointRepo _repository;
        private readonly IGeneralDataSectionRepo _generalDataSectionRepo;
        private readonly IMarkRepo _markRepo;

        public MarkGeneralDataPointService(IMarkGeneralDataPointRepo markGeneralDataPointRepo,
            IGeneralDataSectionRepo generalDataPointRepo,
            IMarkRepo markRepo)
        {
            _repository = markGeneralDataPointRepo;
            _generalDataSectionRepo = generalDataPointRepo;
            _markRepo = markRepo;
        }

        public IEnumerable<MarkGeneralDataPoint> GetAllByMarkAndSectionId(
            int markId, int sectionId)
        {
            return _repository.GetAllByMarkAndSectionId(markId, sectionId);
        }

        public void Create(
            MarkGeneralDataPoint markGeneralDataPoint,
            int markId,
            int sectionId)
        {
            if (markGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(markGeneralDataPoint));
            var foundSection = _generalDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));
            var foundmark = _markRepo.GetById(markId);
            if (foundmark == null)
                throw new ArgumentNullException(nameof(foundmark));

            var uniqueConstraintViolationCheck = _repository.GetByMarkAndSectionIdAndText(
                markId, sectionId, markGeneralDataPoint.Text);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            markGeneralDataPoint.Section = foundSection;
            markGeneralDataPoint.Mark = foundmark;

            int maxNum = 0;
            foreach (var p in _repository.GetAllByMarkAndSectionId(markId, sectionId))
            {
                if (p.OrderNum > maxNum)
                    maxNum = p.OrderNum;
            }
            markGeneralDataPoint.OrderNum = maxNum + 1;
            
            _repository.Add(markGeneralDataPoint);
        }

        public void Update(
            int id,
            MarkGeneralDataPointUpdateRequest markGeneralDataPoint)
        {
            if (markGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(markGeneralDataPoint));
            var foundMarkGeneralDataPoint = _repository.GetById(id);
            if (foundMarkGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundMarkGeneralDataPoint));

            if (markGeneralDataPoint.Text != null)
            {
                var uniqueConstraintViolationCheck = _repository.GetByMarkAndSectionIdAndText(
                    foundMarkGeneralDataPoint.Mark.Id,
                    foundMarkGeneralDataPoint.Section.Id,
                    markGeneralDataPoint.Text);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
                foundMarkGeneralDataPoint.Text = markGeneralDataPoint.Text;
            }
            if (markGeneralDataPoint.OrderNum != null)
            {
                var orderNum = markGeneralDataPoint.OrderNum.GetValueOrDefault();
                var uniqueConstraintViolationCheck = _repository.GetByMarkAndSectionIdAndOrderNum(
                    foundMarkGeneralDataPoint.Mark.Id, foundMarkGeneralDataPoint.Section.Id, orderNum);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
                foundMarkGeneralDataPoint.OrderNum = orderNum;
            }

            _repository.Update(foundMarkGeneralDataPoint);
        }

        public void Delete(int id)
        {
            var foundMarkGeneralDataPoint = _repository.GetById(id);
            if (foundMarkGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundMarkGeneralDataPoint));
            _repository.Delete(foundMarkGeneralDataPoint);
        }

        public IEnumerable<GeneralDataSection> GetSectionsByMarkId(int markId)
        {
            return _repository.GetSectionsByMarkId(markId);
        }
    }
}
