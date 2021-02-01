using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class BoltDiameterService : IBoltDiameterService
    {
        private readonly IBoltDiameterRepo _repository;

        public BoltDiameterService(IBoltDiameterRepo boltDiameterRepo)
        {
            _repository = boltDiameterRepo;
        }

        public IEnumerable<BoltDiameter> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
