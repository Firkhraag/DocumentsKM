using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using System.Linq;

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
    }
}
