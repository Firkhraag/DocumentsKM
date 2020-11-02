using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using Serilog;
using System.Linq;

namespace DocumentsKM.Services
{
    public class BasicSheetService : IBasicSheetService
    {
        private ISheetRepo _repository;
        private readonly IMarkRepo _markRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IDocTypeRepo _docTypeRepo;

        public BasicSheetService(
            ISheetRepo sheetRepo,
            IMarkRepo markRepo,
            IEmployeeRepo employeeRepo,
            IDocTypeRepo docTypeRepo)
        {
            _repository = sheetRepo;
            _markRepo = markRepo;
            _employeeRepo = employeeRepo;
            _docTypeRepo = docTypeRepo;
        }

        public IEnumerable<Sheet> GetAllByMarkId(int markId)
        {
            return _repository.GetAllByMarkIdAndDocType(markId, 1);
        }

        public void Create(
            Sheet sheet,
            int markId,
            int? creatorId,
            int? inspectorId,
            int? normContrId)
        {
            if (sheet == null)
                throw new ArgumentNullException(nameof(sheet));
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
            sheet.Mark = foundMark;

            // Doc type is 1 - лист основного комплекта
            var docType = _docTypeRepo.GetById(1);
            sheet.DocType = docType;

            var sheets = _repository.GetAllByMarkIdAndDocType(markId, 1);
            int maxNum = 1;
            foreach (var s in sheets)
            {
                if (s.Num > maxNum)
                    maxNum = s.Num;
            }
            sheet.Num = maxNum + 1;

            if (creatorId != null)
            {
                var creator = _employeeRepo.GetById(creatorId.GetValueOrDefault());
                if (creator == null)
                    throw new ArgumentNullException(nameof(creator));
                sheet.Creator = creator;
            }
            if (inspectorId != null)
            {
                var inspector = _employeeRepo.GetById(inspectorId.GetValueOrDefault());
                if (inspector == null)
                    throw new ArgumentNullException(nameof(inspector));
                sheet.Inspector = inspector;
            }
            if (normContrId != null)
            {
                var normContr = _employeeRepo.GetById(normContrId.GetValueOrDefault());
                if (normContr == null)
                    throw new ArgumentNullException(nameof(normContr));
                sheet.NormContr = normContr;
            }
            // Log.Information(sheet.Creator.ToString());
            _repository.Add(sheet);
        }

        public void Update(
            int id,
            SheetUpdateRequest sheet)
        {
            if (sheet == null)
                throw new ArgumentNullException(nameof(sheet));
            var foundSheet = _repository.GetById(id);
            if (foundSheet == null)
                throw new ArgumentNullException(nameof(foundSheet));
            if (sheet.Num != null)
                foundSheet.Num = sheet.Num.GetValueOrDefault();
            if (sheet.Name != null)
                foundSheet.Name = sheet.Name;
            if (sheet.Form != null)
                foundSheet.Form = sheet.Form.GetValueOrDefault();
            if (sheet.Note != null)
                foundSheet.Note = sheet.Note;
            if (sheet.CreatorId != null)
            {
                var creatorId = sheet.CreatorId.GetValueOrDefault();
                if (creatorId == -1)
                    foundSheet.Creator = null;
                else
                {
                    var creator = _employeeRepo.GetById(creatorId);
                    if (creator == null)
                        throw new ArgumentNullException(nameof(creator));
                    foundSheet.Creator = creator;
                }
                
            }
            if (sheet.InspectorId != null)
            {
                var inspectorId = sheet.InspectorId.GetValueOrDefault();
                if (inspectorId == -1)
                    foundSheet.Inspector = null;
                else
                {
                    var inspector = _employeeRepo.GetById(inspectorId);
                    if (inspector == null)
                        throw new ArgumentNullException(nameof(inspector));
                    foundSheet.Inspector = inspector;
                }
                
            }
            if (sheet.NormContrId != null)
            {
                var normContrId = sheet.NormContrId.GetValueOrDefault();
                if (normContrId == -1)
                    foundSheet.NormContr = null;
                else
                {
                    var normContr = _employeeRepo.GetById(normContrId);
                    if (normContr == null)
                        throw new ArgumentNullException(nameof(normContr));
                    foundSheet.NormContr = normContr;
                }
                
            }
            _repository.Update(foundSheet);
        }
    }
}
