using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class WeldingControlService : IWeldingControlService
    {
        private IWeldingControlRepo _repository;

        public WeldingControlService(IWeldingControlRepo weldingControlRepoo)
        {
            _repository = weldingControlRepoo;
        }

        public IEnumerable<WeldingControl> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
