using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class ProjectService : IProjectService
    {
        private IProjectRepo _repository;

        public ProjectService(IProjectRepo ProjectRepo)
        {
            _repository = ProjectRepo;
        }

        public IEnumerable<Project> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
