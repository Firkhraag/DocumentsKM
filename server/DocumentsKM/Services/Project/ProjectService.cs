using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo _repository;

        public ProjectService(IProjectRepo projectRepo)
        {
            _repository = projectRepo;
        }

        public IEnumerable<Project> GetAll()
        {
            return _repository.GetAll();
        }

        public Project GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}
