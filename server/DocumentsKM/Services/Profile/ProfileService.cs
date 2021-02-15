using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepo _repository;

        public ProfileService(IProfileRepo profileRepo)
        {
            _repository = profileRepo;
        }

        public IEnumerable<Profile> GetAllByProfileClassId(int profileClassId)
        {
            return _repository.GetAllByProfileClassId(profileClassId);
        }
    }
}
