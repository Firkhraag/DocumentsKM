using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class PaintworkTypeService : IPaintworkTypeService
    {
        private IPaintworkTypeRepo _repository;

        public PaintworkTypeService(IPaintworkTypeRepo paintworkTypeRepoo)
        {
            _repository = paintworkTypeRepoo;
        }

        public IEnumerable<PaintworkType> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
