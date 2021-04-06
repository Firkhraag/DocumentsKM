using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using System.Linq;

namespace DocumentsKM.Services
{
    public class AttachedDocService : IAttachedDocService
    {
        private readonly IAttachedDocRepo _repository;
        private readonly IMarkRepo _markRepo;

        public AttachedDocService(
            IAttachedDocRepo attachedDocRepo,
            IMarkRepo markRepo)
        {
            _repository = attachedDocRepo;
            _markRepo = markRepo;
        }

        public IEnumerable<AttachedDoc> GetAllByMarkId(int markId)
        {
            return _repository.GetAllByMarkId(markId);
        }

        public string GetNewDesignation(int markId){
            var designations = _repository.GetAllByMarkId(markId).Select(v => v.Designation);
            if (designations.Count() == 0)
                return "ЛС";
            return $"ЛС{designations.Count()}";
        }

        public void Create(
            AttachedDoc attachedDoc,
            int markId)
        {
            if (attachedDoc == null)
                throw new ArgumentNullException(nameof(attachedDoc));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));

            var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                markId, attachedDoc.Designation);
            if (uniqueConstraintViolationCheck != null)
                throw new ConflictException(
                    uniqueConstraintViolationCheck.Id.ToString());

            attachedDoc.Mark = foundMark;
            _repository.Add(attachedDoc);

            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public void Update(
            int id,
            AttachedDocUpdateRequest attachedDoc)
        {
            if (attachedDoc == null)
                throw new ArgumentNullException(nameof(attachedDoc));
            var foundAttachedDoc = _repository.GetById(id);
            if (foundAttachedDoc == null)
                throw new ArgumentNullException(nameof(foundAttachedDoc));

            if (attachedDoc.Designation != null)
            {
                var uniqueConstraintViolationCheck = _repository.GetByUniqueKey(
                    foundAttachedDoc.Mark.Id, attachedDoc.Designation);
                if (uniqueConstraintViolationCheck != null && uniqueConstraintViolationCheck.Id != id)
                    throw new ConflictException(uniqueConstraintViolationCheck.Id.ToString());
                foundAttachedDoc.Designation = attachedDoc.Designation;
            }
            if (attachedDoc.Name != null)
                foundAttachedDoc.Name = attachedDoc.Name;
            if (attachedDoc.Note != null)
                foundAttachedDoc.Note = attachedDoc.Note;

            _repository.Update(foundAttachedDoc);

            var foundMark = _markRepo.GetById(foundAttachedDoc.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public void Delete(int id)
        {
            var foundAttachedDoc = _repository.GetById(id);
            if (foundAttachedDoc == null)
                throw new ArgumentNullException(nameof(foundAttachedDoc));
            var markId = foundAttachedDoc.Mark.Id;
            _repository.Delete(foundAttachedDoc);

            var foundMark = _markRepo.GetById(markId);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }
    }
}
