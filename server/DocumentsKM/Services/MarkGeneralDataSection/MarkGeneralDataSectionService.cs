using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using System.Linq;

namespace DocumentsKM.Services
{
    public class MarkGeneralDataSectionService : IMarkGeneralDataSectionService
    {
        private readonly IMarkGeneralDataSectionRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly IGeneralDataSectionRepo _generalDataSectionRepo;

        public MarkGeneralDataSectionService(
            IMarkGeneralDataSectionRepo markGeneralDataSectionRepo,
            IMarkRepo markRepo,
            IGeneralDataSectionRepo generalDataSectionRepo,
            IGeneralDataPointRepo generalDataPointRepo)
        {
            _repository = markGeneralDataSectionRepo;
            _markRepo = markRepo;
            _generalDataSectionRepo = generalDataSectionRepo;
        }

        public IEnumerable<MarkGeneralDataSection> GetAllByMarkId(int markId)
        {
            return _repository.GetAllByMarkId(markId);
        }

        public void Create(
            MarkGeneralDataSection markGeneralDataSection,
            int markId)
        {
            if (markGeneralDataSection == null)
                throw new ArgumentNullException(nameof(markGeneralDataSection));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                markId, markGeneralDataSection.Name);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(nameof(uniqueConstraintViolationCheck));

            markGeneralDataSection.Mark = foundMark;

            var currentSections = _repository.GetAllByMarkId(markId);
            if (currentSections.Count() == 0)
                markGeneralDataSection.OrderNum = 1;
            else
                markGeneralDataSection.OrderNum = (Int16)(currentSections.Max(v => v.OrderNum) + 1);

            _repository.Add(markGeneralDataSection);

            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public IEnumerable<MarkGeneralDataSection> UpdateAllBySectionIds(int userId, int markId, List<int> sectionIds)
        {
            if (sectionIds == null)
                throw new ArgumentNullException(nameof(sectionIds));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));

            var currentSections = _repository.GetAllByMarkId(markId).ToList();

            var generalDataSections = new List<GeneralDataSection> { };
            foreach (var id in sectionIds)
            {
                var generalDataSection = _generalDataSectionRepo.GetById(id, true);
                if (generalDataSection == null)
                    throw new ArgumentNullException(nameof(generalDataSection));
                generalDataSections.Add(generalDataSection);
            }

            var allUserSections = _generalDataSectionRepo.GetAllByUserId(userId);
            foreach (var userSection in allUserSections)
                if (!sectionIds.Contains(userSection.Id))
                    if (currentSections.Select(v => v.Name).Contains(userSection.Name))
                    {
                        var section = currentSections.SingleOrDefault(v => v.Name == userSection.Name);
                        var p = section.MarkGeneralDataPoints;
                        _repository.Delete(
                            currentSections.SingleOrDefault(v => v.Name == userSection.Name));
                        currentSections.Remove(section);
                    }

            foreach (var s in generalDataSections.OrderBy(v => v.OrderNum))
            {
                var uniqueConstraintCheck = _repository.GetByUniqueKey(
                    markId, s.Name);
                if (uniqueConstraintCheck == null)
                {
                    var markGeneralDataSection = new MarkGeneralDataSection
                    {
                        Mark = foundMark,
                        Name = s.Name,
                    };
                    if (currentSections.Count() == 0)
                        markGeneralDataSection.OrderNum = 1;
                    else
                        markGeneralDataSection.OrderNum = (Int16)(currentSections.Max(v => v.OrderNum) + 1);

                    var markGeneralDataPoints = new List<MarkGeneralDataPoint>(){};
                    foreach (var p in s.GeneralDataPoints)
                    {
                        markGeneralDataPoints.Add(new MarkGeneralDataPoint
                        {
                            Section = markGeneralDataSection,
                            Text = p.Text,
                            OrderNum = p.OrderNum,
                        });
                    }
                    markGeneralDataSection.MarkGeneralDataPoints = markGeneralDataPoints;
                    
                    _repository.Add(markGeneralDataSection);
                    currentSections.Add(markGeneralDataSection);
                }
            }
            short num = 1;
            foreach (var p in currentSections)
            {
                p.OrderNum = num;
                _repository.Update(p);
                num += 1;
            }

            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
            
            return currentSections;
        }

        public void Update(
            int id, int markId,
            MarkGeneralDataSectionUpdateRequest markGeneralDataSection)
        {
            if (markGeneralDataSection == null)
                throw new ArgumentNullException(nameof(markGeneralDataSection));
            var foundMarkGeneralDataSection = _repository.GetById(id);
            if (foundMarkGeneralDataSection == null)
                throw new ArgumentNullException(nameof(foundMarkGeneralDataSection));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));

            if (markGeneralDataSection.Name != null)
            {
                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                    foundMarkGeneralDataSection.Mark.Id,
                    markGeneralDataSection.Name);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(nameof(uniqueConstraintViolationCheck));
                foundMarkGeneralDataSection.Name = markGeneralDataSection.Name;
            }
            if (markGeneralDataSection.OrderNum != null)
            {
                var orderNum = markGeneralDataSection.OrderNum.GetValueOrDefault();
                foundMarkGeneralDataSection.OrderNum = orderNum;
                short num = 1;
                foreach (var p in _repository.GetAllByMarkId(markId))
                {
                    if (p.Id == id)
                        continue;
                    if (num == markGeneralDataSection.OrderNum)
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

            _repository.Update(foundMarkGeneralDataSection);

            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public void Delete(int id, int markId)
        {
            var foundMarkGeneralDataSection = _repository.GetById(id, true);
            if (foundMarkGeneralDataSection == null)
                throw new ArgumentNullException(nameof(foundMarkGeneralDataSection));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            foreach (var p in _repository.GetAllByMarkId(markId))
            {
                if (p.OrderNum > foundMarkGeneralDataSection.OrderNum)
                {
                    p.OrderNum = (Int16)(p.OrderNum - 1);
                    _repository.Update(p);
                }
            }
            _repository.Delete(foundMarkGeneralDataSection);

            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }
    }
}
