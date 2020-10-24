using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System;
using DocumentsKM.Dtos;
using Serilog;
using System.Linq;

namespace DocumentsKM.Services
{
    public class SheetService : ISheetService
    {
        private ISheetRepo _repository;
        private readonly IMarkRepo _markRepo;

        public SheetService(
            ISheetRepo sheetRepo,
            IMarkRepo markRepo)
        {
            _repository = sheetRepo;
            _markRepo = markRepo;
        }

        public IEnumerable<Sheet> GetAllByMarkId(int markId)
        {
            return _repository.GetAllByMarkId(markId);
        }

        public void Create(
            int markId,
            string note)
        {
            var foundMark = _markRepo.GetById(markId);
            if (foundMark == null)
                throw new ArgumentNullException(nameof(foundMark));
        }
    }
}
