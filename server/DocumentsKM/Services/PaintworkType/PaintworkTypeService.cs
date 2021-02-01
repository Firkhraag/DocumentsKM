using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class PaintworkTypeService : IPaintworkTypeService
    {
        private readonly IPaintworkTypeRepo _repository;

        public PaintworkTypeService(IPaintworkTypeRepo paintworkTypeRepo)
        {
            _repository = paintworkTypeRepo;
        }

        public IEnumerable<PaintworkType> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
