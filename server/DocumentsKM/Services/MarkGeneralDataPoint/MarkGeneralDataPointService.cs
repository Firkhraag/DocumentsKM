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
        private readonly IMarkGeneralDataPointRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly IMarkGeneralDataSectionRepo _markGeneralDataSectionRepo;
        private readonly IGeneralDataPointRepo _generalDataPointRepo;
        private readonly IGeneralDataSectionRepo _generalDataSectionRepo;

        public MarkGeneralDataPointService(
            IMarkGeneralDataPointRepo markGeneralDataPointRepo,
            IMarkRepo markRepo,
            IMarkGeneralDataSectionRepo markGeneralDataSectionRepo,
            IGeneralDataPointRepo generalDataPointRepo,
            IGeneralDataSectionRepo generalDataSectionRepo)
        {
            _repository = markGeneralDataPointRepo;
            _markRepo = markRepo;
            _markGeneralDataSectionRepo = markGeneralDataSectionRepo;
            _generalDataPointRepo = generalDataPointRepo;
            _generalDataSectionRepo = generalDataSectionRepo;
        }

        public IEnumerable<MarkGeneralDataPoint> GetAllBySectionId(
            int sectionId)
        {
            return _repository.GetAllBySectionId(sectionId);
        }

        public void Create(
            MarkGeneralDataPoint markGeneralDataPoint,
            int sectionId)
        {
            if (markGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(markGeneralDataPoint));
            var foundSection = _markGeneralDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                sectionId, markGeneralDataPoint.Text);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            markGeneralDataPoint.Section = foundSection;

            var currentPoints = _repository.GetAllBySectionId(sectionId);
            if (currentPoints.Count() == 0)
                markGeneralDataPoint.OrderNum = 1;
            else
                markGeneralDataPoint.OrderNum = (Int16)(currentPoints.Max(v => v.OrderNum) + 1);

            _repository.Add(markGeneralDataPoint);

            var mark = foundSection.Mark;
            mark.EditedDate = DateTime.Now;
            _markRepo.Update(mark);
        }

        public IEnumerable<MarkGeneralDataPoint> UpdateAllByPointIds(
            int userId, int sectionId, List<int> pointIds)
        {
            if (pointIds == null)
                throw new ArgumentNullException(nameof(pointIds));
            var foundSection = _markGeneralDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));

            var currentPoints = _repository.GetAllBySectionId(
                sectionId).ToList();

            var generalDataPoints = new List<GeneralDataPoint> { };
            foreach (var id in pointIds)
            {
                var generalDataPoint = _generalDataPointRepo.GetById(id);
                if (generalDataPoint == null)
                    throw new ArgumentNullException(nameof(generalDataPoint));
                generalDataPoints.Add(generalDataPoint);
            }

            var allUserPoints = _generalDataPointRepo.GetAllBySectionId(sectionId);
            foreach (var userPoint in allUserPoints)
                if (!pointIds.Contains(userPoint.Id))
                    if (currentPoints.Select(v => v.Text).Contains(userPoint.Text))
                    {
                        var point = currentPoints.SingleOrDefault(v => v.Text == userPoint.Text);
                        _repository.Delete(
                            currentPoints.SingleOrDefault(v => v.Text == userPoint.Text));
                        currentPoints.Remove(point);
                    }

            foreach (var p in generalDataPoints.OrderBy(v => v.OrderNum))
            {
                var uniqueConstraintCheck = _repository.GetByUniqueKey(
                    sectionId, p.Text);
                if (uniqueConstraintCheck == null)
                {
                    var markGeneralDataPoint = new MarkGeneralDataPoint
                    {
                        Section = foundSection,
                        Text = p.Text,
                    };
                    if (currentPoints.Count() == 0)
                        markGeneralDataPoint.OrderNum = 1;
                    else
                        markGeneralDataPoint.OrderNum = (Int16)(currentPoints.Max(v => v.OrderNum) + 1);
                    _repository.Add(markGeneralDataPoint);
                    currentPoints.Add(markGeneralDataPoint);
                }
            }
            short num = 1;
            foreach (var p in currentPoints)
            {
                p.OrderNum = num;
                _repository.Update(p);
                num += 1;
            }

            var mark = foundSection.Mark;
            mark.EditedDate = DateTime.Now;
            _markRepo.Update(mark);
            
            return currentPoints;
        }

        public void Update(
            int id, int sectionId,
            MarkGeneralDataPointUpdateRequest markGeneralDataPoint)
        {
            if (markGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(markGeneralDataPoint));
            var foundMarkGeneralDataPoint = _repository.GetById(id);
            if (foundMarkGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundMarkGeneralDataPoint));
            var foundSection = _markGeneralDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));

            if (markGeneralDataPoint.Text != null)
            {
                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
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
                short num = 1;
                foreach (var p in _repository.GetAllBySectionId(sectionId))
                {
                    if (p.Id == id)
                        continue;
                    if (num == markGeneralDataPoint.OrderNum)
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

            _repository.Update(foundMarkGeneralDataPoint);

            var mark = foundSection.Mark;
            mark.EditedDate = DateTime.Now;
            _markRepo.Update(mark);
        }

        public void Delete(int id, int sectionId)
        {
            var foundMarkGeneralDataPoint = _repository.GetById(id);
            if (foundMarkGeneralDataPoint == null)
                throw new ArgumentNullException(nameof(foundMarkGeneralDataPoint));
            var foundSection = _markGeneralDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));
            foreach (var p in _repository.GetAllBySectionId(sectionId))
            {
                if (p.OrderNum > foundMarkGeneralDataPoint.OrderNum)
                {
                    p.OrderNum = (Int16)(p.OrderNum - 1);
                    _repository.Update(p);
                }
            }
            _repository.Delete(foundMarkGeneralDataPoint);

            var mark = foundSection.Mark;
            mark.EditedDate = DateTime.Now;
            _markRepo.Update(mark);
        }

        public void AddDefaultPoints(int userId, Mark mark)
        {
            var sections = _generalDataSectionRepo.GetAllByUserId(userId);
            foreach (var section in sections)
            {
                var markSection = new MarkGeneralDataSection()
                {
                    Mark = mark,
                    Name = section.Name,
                    OrderNum = section.OrderNum,
                };

                _markGeneralDataSectionRepo.Add(markSection);

                var points = _generalDataPointRepo.GetAllBySectionId(section.Id);
                var markPoints = points.Select(v => new MarkGeneralDataPoint()
                {
                    Section = markSection,
                    Text = v.Text,
                    OrderNum = v.OrderNum,
                });
                foreach (var markPoint in markPoints)
                {
                    _repository.Add(markPoint);
                }
            }
        }
    }
}
