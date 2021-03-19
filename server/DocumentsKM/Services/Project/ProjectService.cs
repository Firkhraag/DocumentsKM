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
            // Delete should be cascade if it's necessary
            // foreach (var project in projects)
            // {
            //     if (!projectsFetched.Select(v => v.Id).Contains(project.Id))
            //         _repository.Delete(project);
            // }
            foreach (var projectFetched in projectsFetched)
            {
                var foundProject = projects.SingleOrDefault(v => v.Id == projectFetched.Id);
                if (foundProject == null)
                    _repository.Add(projectFetched.ToProject());
                else
                {
                    var wasChanged = false;
                    if (foundProject.Name != projectFetched.Name)
                    {
                        foundProject.Name = projectFetched.Name;
                        wasChanged = true;
                    }
                    if (wasChanged)
                        _repository.Update(foundProject);
                }
            }
        }
    }
}
