using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class WeldingControlService : IWeldingControlService
    {
        private readonly IWeldingControlRepo _repository;

        public WeldingControlService(IWeldingControlRepo weldingControlRepo)
        {
            _repository = weldingControlRepo;
        }

        public IEnumerable<WeldingControl> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
