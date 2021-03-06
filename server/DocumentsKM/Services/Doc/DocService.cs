using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class DocService : IDocService
    {
        // Id листа основного комплекта из справочника типов документов
        private readonly int _sheetDocTypeId = 1;

        private readonly IDocRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IDocTypeRepo _docTypeRepo;

        public DocService(
            IDocRepo docRepo,
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo,
            IDocTypeRepo docTypeRepo)
        {
            _repository = docRepo;
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
            _docTypeRepo = docTypeRepo;
        }

        public IEnumerable<Doc> GetAllByMarkId(int markId)
        {
            return _repository.GetAllByMarkId(markId);
        }

        public IEnumerable<Doc> GetAllSheetsByMarkId(int markId)
        {
            return _repository.GetAllByMarkIdAndDocType(
                markId, _sheetDocTypeId);
        }

        public IEnumerable<Doc> GetAllAttachedByMarkId(int markId)
        {
            return _repository.GetAllByMarkIdAndNotDocType(
                markId, _sheetDocTypeId);
        }

        public void Create(
            Doc doc,
            int markId,
            int docTypeId,
            int creatorId,
            int? inspectorId,
            int? normContrId)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            doc.Mark = foundMark;

            var foundDocType = _docTypeRepo.GetById(docTypeId);
            if (foundDocType == null)
                throw new ArgumentNullException(nameof(foundDocType));
            doc.Type = foundDocType;

            var docs = _repository.GetAllByMarkIdAndDocType(markId, docTypeId);
            int maxNum = 0;
            foreach (var s in docs)
            {
                if (s.Num > maxNum)
                    maxNum = s.Num;
            }
            doc.Num = (Int16)(maxNum + 1);

            var creator = _employeeRepo.GetById(creatorId);
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));
            doc.Creator = creator;
            if (inspectorId != null)
            {
                var inspector = _employeeRepo.GetById(inspectorId.GetValueOrDefault());
                if (inspector == null)
                    throw new ArgumentNullException(nameof(inspector));
                doc.Inspector = inspector;
            }
            if (normContrId != null)
            {
                var normContr = _employeeRepo.GetById(normContrId.GetValueOrDefault());
                if (normContr == null)
                    throw new ArgumentNullException(nameof(normContr));
                doc.NormContr = normContr;
            }
            _repository.Add(doc);

            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public void Update(
            int id,
            DocUpdateRequest doc)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));
            var foundDoc = _repository.GetById(id);
            if (foundDoc == null)
                throw new ArgumentNullException(nameof(foundDoc));
            if (doc.Name != null)
                foundDoc.Name = doc.Name;
            if (doc.Form != null)
                foundDoc.Form = doc.Form.GetValueOrDefault();
            if (doc.ReleaseNum != null)
                foundDoc.ReleaseNum = doc.ReleaseNum.GetValueOrDefault();
            if (doc.NumOfPages != null)
                foundDoc.NumOfPages = doc.NumOfPages.GetValueOrDefault();
            if (doc.Note != null)
                foundDoc.Note = doc.Note;
            if (doc.TypeId != null)
            {

                var docs = _repository.GetAllByMarkIdAndDocType(
                    foundDoc.Mark.Id, doc.TypeId.GetValueOrDefault());
                int maxNum = 0;
                foreach (var s in docs)
                {
                    if (s.Num > maxNum)
                        maxNum = s.Num;
                }
                foundDoc.Num = (Int16)(maxNum + 1);

                var docType = _docTypeRepo.GetById(doc.TypeId.GetValueOrDefault());
                if (docType == null)
                    throw new ArgumentNullException(nameof(docType));
                foundDoc.Type = docType;
            }
            if (doc.CreatorId != null)
            {
                var creator = _employeeRepo.GetById(
                    doc.CreatorId.GetValueOrDefault());
                if (creator == null)
                    throw new ArgumentNullException(nameof(creator));
                foundDoc.Creator = creator;
            }
            if (doc.InspectorId != null)
            {
                var inspectorId = doc.InspectorId.GetValueOrDefault();
                if (inspectorId == -1)
                    foundDoc.InspectorId = null;
                else
                {
                    var inspector = _employeeRepo.GetById(inspectorId);
                    if (inspector == null)
                        throw new ArgumentNullException(nameof(inspector));
                    foundDoc.Inspector = inspector;
                }

            }
            if (doc.NormContrId != null)
            {
                var normContrId = doc.NormContrId.GetValueOrDefault();
                if (normContrId == -1)
                    foundDoc.NormContrId = null;
                else
                {
                    var normContr = _employeeRepo.GetById(normContrId);
                    if (normContr == null)
                        throw new ArgumentNullException(nameof(normContr));
                    foundDoc.NormContr = normContr;
                }

            }
            _repository.Update(foundDoc);

            var foundMark = _markRepo.GetById(foundDoc.Mark.Id);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }

        public void Delete(int id)
        {
            var foundDoc = _repository.GetById(id);
            if (foundDoc == null)
                throw new ArgumentNullException(nameof(foundDoc));
            var markId = foundDoc.Mark.Id;
            _repository.Delete(foundDoc);

            var foundMark = _markRepo.GetById(markId);
            foundMark.EditedDate = DateTime.Now;
            _markRepo.Update(foundMark);
        }
    }
}
