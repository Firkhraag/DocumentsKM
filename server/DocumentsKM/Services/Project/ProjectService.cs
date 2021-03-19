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

        public void UpdateAll(List<ArchiveProject> projectsFetched)
        {
            var projects = _repository.GetAll();
            foreach (var project in projects)
            {
                if (!projectsFetched.Select(v => v.Id).Contains(project.Id))
                    _repository.Delete(project);
            }
            foreach (var projectFetched in projectsFetched)
            {
                var foundDepartment = projects.SingleOrDefault(v => v.Id == projectFetched.Id);
                if (foundDepartment == null)
                    _repository.Add(projectFetched.ToProject());
                else
                {
                    var wasChanged = false;
                    if (foundDepartment.Name != projectFetched.Name)
                    {
                        foundDepartment.Name = projectFetched.Name;
                        wasChanged = true;
                    }
                    if (wasChanged)
                        _repository.Update(foundDepartment);
                }
            }
        }
    }
}
