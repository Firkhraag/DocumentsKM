using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class ProfileClassService : IProfileClassService
    {
        private readonly IProfileClassRepo _repository;

        public ProfileClassService(IProfileClassRepo profileClassRepo)
        {
            _repository = profileClassRepo;
        }

        public IEnumerable<ProfileClass> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
