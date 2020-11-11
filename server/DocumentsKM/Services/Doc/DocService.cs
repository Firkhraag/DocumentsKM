using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using Serilog;
using System.Linq;

namespace DocumentsKM.Services
{
    public class DocService : IDocService
    {
        // Id листа основного комплекта из справочника типов документов
        private readonly int _sheetDocTypeId = 1;

        private IDocRepo _repository;
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

        public IEnumerable<Doc> GetAllSheetsByMarkId(int markId)
        {
            return _repository.GetAllByMarkIdAndDocType(markId, _sheetDocTypeId);
        }

        public IEnumerable<Doc> GetAllAttachedByMarkId(int markId)
        {
            return _repository.GetAllByMarkIdAndNotDocType(markId, _sheetDocTypeId);
        }

        public void Create(
            Doc doc,
            int markId,
            int docTypeId,
            int? creatorId,
            int? inspectorId,
            int? normContrId)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            doc.Mark = foundMark;

            var docType = _docTypeRepo.GetById(docTypeId);
            doc.Type = docType;

            var docs = _repository.GetAllByMarkIdAndDocType(markId, docTypeId);
            int maxNum = 1;
            foreach (var s in docs)
            {
                if (s.Num > maxNum)
                    maxNum = s.Num;
            }
            doc.Num = maxNum + 1;

            if (creatorId != null)
            {
                var creator = _employeeRepo.GetById(creatorId.GetValueOrDefault());
                if (creator == null)
                    throw new ArgumentNullException(nameof(creator));
                doc.Creator = creator;
            }
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
            // Log.Information(doc.Creator.ToString());
            _repository.Add(doc);
        }

        public void Update(
            int id,
            DocUpdateRequest doc)
        {
            // ToDo: Конфликты по юник ки
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));
            var foundDoc = _repository.GetById(id);
            if (foundDoc == null)
                throw new ArgumentNullException(nameof(foundDoc));
            if (doc.Num != null)
                foundDoc.Num = doc.Num.GetValueOrDefault();
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
            // Maybe type?
            if (doc.CreatorId != null)
            {
                var creatorId = doc.CreatorId.GetValueOrDefault();
                if (creatorId == -1)
                    foundDoc.Creator = null;
                else
                {
                    var creator = _employeeRepo.GetById(creatorId);
                    if (creator == null)
                        throw new ArgumentNullException(nameof(creator));
                    foundDoc.Creator = creator;
                }
                
            }
            if (doc.InspectorId != null)
            {
                var inspectorId = doc.InspectorId.GetValueOrDefault();
                if (inspectorId == -1)
                    foundDoc.Inspector = null;
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
                    foundDoc.NormContr = null;
                else
                {
                    var normContr = _employeeRepo.GetById(normContrId);
                    if (normContr == null)
                        throw new ArgumentNullException(nameof(normContr));
                    foundDoc.NormContr = normContr;
                }
                
            }
            _repository.Update(foundDoc);
        }

        public void Delete(int id)
        {
            var foundDoc = _repository.GetById(id);
            if (foundDoc == null)
                throw new ArgumentNullException(nameof(foundDoc));
            _repository.Delete(foundDoc);
        }
    }
}