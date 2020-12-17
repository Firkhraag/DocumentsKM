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
        private readonly IGeneralDataPointRepo _generalDataPointRepo;
        private readonly IMarkRepo _markRepo;

        public MarkGeneralDataPointService(IMarkGeneralDataPointRepo markGeneralDataPointRepo,
            IGeneralDataSectionRepo generalDataSectionRepo,
            IGeneralDataPointRepo generalDataPointRepo,
            IMarkRepo markRepo)
        {
            _repository = markGeneralDataPointRepo;
            _generalDataSectionRepo = generalDataSectionRepo;
            _generalDataPointRepo = generalDataPointRepo;
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
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));

            var uniqueConstraintViolationCheck = _repository.GetByMarkAndSectionIdAndText(
                markId, sectionId, markGeneralDataPoint.Text);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());

            markGeneralDataPoint.Section = foundSection;
            markGeneralDataPoint.Mark = foundMark;

            var currentPoints = _repository.GetAllByMarkAndSectionId(markId, sectionId);
            if (currentPoints.Count() == 0)
                markGeneralDataPoint.OrderNum = 1;
            else
                markGeneralDataPoint.OrderNum = currentPoints.Max(v => v.OrderNum) + 1;
            
            _repository.Add(markGeneralDataPoint);
        }

        public void UpdateAllBySectionIds(int markId, List<int> sectionIds)
        {
            if (sectionIds == null)
                throw new ArgumentNullException(nameof(sectionIds));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));

            foreach (var id in sectionIds)
            {
                var section = _generalDataSectionRepo.GetById(id);
                if (section == null)
                    throw new ArgumentNullException(nameof(section));
            }

            var points = _repository.GetAllByMarkId(markId);
            var currentPointIds = new List<int>{};
            foreach (var p in points)
            {
                if (!sectionIds.Contains(p.Section.Id))
                    _repository.Delete(p);
                currentPointIds.Add(p.Section.Id);
            }
        }

        public IEnumerable<MarkGeneralDataPoint> UpdateAllByPointIds(int markId, int sectionId, List<int> pointIds)
        {
            if (pointIds == null)
                throw new ArgumentNullException(nameof(pointIds));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            var foundSection = _generalDataSectionRepo.GetById(sectionId);
            if (foundSection == null)
                throw new ArgumentNullException(nameof(foundSection));

            var addedMarkGeneralDataPoints = new List<MarkGeneralDataPoint>{};

            var generalDataPoints = new List<GeneralDataPoint>{};
            foreach (var id in pointIds)
            {
                var generalDataPoint = _generalDataPointRepo.GetById(id);
                if (generalDataPoint == null)
                    throw new ArgumentNullException(nameof(generalDataPoint));
                generalDataPoints.Add(generalDataPoint);
            }

            foreach (var p in generalDataPoints.OrderBy(v => v.OrderNum))
            {
                var uniqueConstraintCheck = _repository.GetByMarkAndSectionIdAndText(
                    markId, sectionId, p.Text);
                if (uniqueConstraintCheck == null)
                {
                    var markGeneralDataPoint = new MarkGeneralDataPoint
                    {
                        Mark = foundMark,
                        Section = foundSection,
                        Text = p.Text,
                    };
                    var currentPoints = _repository.GetAllByMarkAndSectionId(markId, sectionId);
                    if (currentPoints.Count() == 0)
                        markGeneralDataPoint.OrderNum = 1;
                    else
                        markGeneralDataPoint.OrderNum = currentPoints.Max(v => v.OrderNum) + 1;
                    _repository.Add(markGeneralDataPoint);
                    addedMarkGeneralDataPoints.Add(markGeneralDataPoint);
                }
            }

            // foreach (var id in pointIds)
            // {
            //     var generalDataPoint = _generalDataPointRepo.GetById(id);
            //     if (generalDataPoint == null)
            //         throw new ArgumentNullException(nameof(generalDataPoint));

            //     var uniqueConstraintCheck = _repository.GetByMarkAndSectionIdAndText(
            //         markId, sectionId, generalDataPoint.Text);
            //     if (uniqueConstraintCheck == null)
            //     {
            //         var markGeneralDataPoint = new MarkGeneralDataPoint
            //         {
            //             Mark = foundMark,
            //             Section = foundSection,
            //             Text = generalDataPoint.Text,
            //         };
            //         var currentPoints = _repository.GetAllByMarkAndSectionId(markId, sectionId);
            //         if (currentPoints.Count() == 0)
            //             markGeneralDataPoint.OrderNum = 1;
            //         else
            //             markGeneralDataPoint.OrderNum = currentPoints.Max(v => v.OrderNum) + 1;
            //         _repository.Add(markGeneralDataPoint);
            //         addedMarkGeneralDataPoints.Add(markGeneralDataPoint);
            //     }
            // }

            return addedMarkGeneralDataPoints;

            // var points = _repository.GetAllByMarkAndSectionId(markId, sectionId);
            // // (double, int) t = (4.5, 3);
            // var currentPoints = new List<(int, string)>{};
            // foreach (var p in points)
            // {
            // //     if (!generalDataPoints.Select(v => v.Text).Contains(p.Text))
            // //         _repository.Delete(p);
            //     currentPoints.Add((p.Id, p.Text));
            // }
            // foreach (var (p, i) in generalDataPoints.WithIndex())
            //     if (!currentPoints.Contains((p.Id, p.Text)))
            //         _repository.Add(
            //             new MarkApproval
            //             {
            //                 Mark=foundMark,
            //                 Employee=employees[i],
            //             });
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
