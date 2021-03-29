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

        public void UpdateAll(List<Project> projectsFetched)
        {
            var projects = _repository.GetAll();
            // Delete should be cascade if it's necessary
            // foreach (var project in projects)
            // {
            //     if (!projectsFetched.Select(v => v.Id).Contains(project.Id))
            //         _repository.Delete(project);
            // }
            
            foreach (var projectFetched in projectsFetched.Where(v => v.Name != null && v.BaseSeries != null))
            {
                var foundProject = projects.SingleOrDefault(v => v.Id == projectFetched.Id);
                if (foundProject == null)
                {
                    var uniqueKeyViolation = _repository.GetByUniqueKey(projectFetched.BaseSeries);
                    if (uniqueKeyViolation == null)
                        _repository.Add(projectFetched);
                }
                else
                {
                    var wasChanged = false;
                    var uniqueKeyWasChanged = false;
                    var name = projectFetched.Name.Trim();
                    if (foundProject.Name != name)
                    {
                        foundProject.Name = name;
                        wasChanged = true;
                    }
                    var baseSeries = projectFetched.BaseSeries.Trim();
                    if (foundProject.Name != baseSeries)
                    {
                        foundProject.BaseSeries = baseSeries;
                        wasChanged = true;
                        uniqueKeyWasChanged = true;
                    }
                    if (wasChanged)
                    {
                        if (uniqueKeyWasChanged)
                        {
                            var uniqueKeyViolation = _repository.GetByUniqueKey(baseSeries);
                            if (uniqueKeyViolation != null)
                                return;
                        }
                        _repository.Update(foundProject);
                    }
                }
            }
        }
    }
}
