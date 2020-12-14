using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using System.Linq;

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

            markGeneralDataPoint.OrderNum = _repository.GetAllByMarkAndSectionId(
                markId, sectionId).Max(v => v.OrderNum) + 1;
            
            _repository.Add(markGeneralDataPoint);
        }

        public void Update(
            int id, int markId, int sectionId,
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
                foundMarkGeneralDataPoint.OrderNum = orderNum;
                var num = 1;
                foreach (var p in _repository.GetAllByMarkAndSectionId(markId, sectionId))
                {
                    if (p.Id == id)
                        continue;
                    if (num == markGeneralDataPoint.OrderNum)
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

            _repository.Update(foundMarkGeneralDataPoint);
        }

        public void Delete(int id, int markId, int sectionId)
        {
            var foundMarkGeneralDataPoint = _repository.GetById(id);
            if (foundMarkGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundMarkGeneralDataPoint));
            foreach (var p in _repository.GetAllByMarkAndSectionId(markId, sectionId))
            {
                if (p.OrderNum > foundMarkGeneralDataPoint.OrderNum)
                {
                    p.OrderNum = p.OrderNum - 1;
                    _repository.Update(p);
                }
            }
            _repository.Delete(foundMarkGeneralDataPoint);
        }

        public IEnumerable<GeneralDataSection> GetSectionsByMarkId(int markId)
        {
            return _repository.GetSectionsByMarkId(markId);
        }
    }
}
