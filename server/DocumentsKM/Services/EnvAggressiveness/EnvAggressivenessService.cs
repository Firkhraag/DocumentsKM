using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class EnvAggressivenessService : IEnvAggressivenessService
    {
        private IEnvAggressivenessRepo _repository;

        public EnvAggressivenessService(
            IEnvAggressivenessRepo envAggressivenessRepo)
        {
            _repository = envAggressivenessRepo;
        }

        public IEnumerable<EnvAggressiveness> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
