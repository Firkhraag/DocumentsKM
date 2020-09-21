using System;
using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    // Repo implementation for testing purposes
    public class MockProjectRepo : IProjectRepo
    {
        // Initial projects list
        private List<Project> _projects = new List<Project>
        {
            new Project
            {
                Id=0,
                BaseSeries="M32788",
            },
            new Project
            {
                Id=1,
                BaseSeries="V14578",
            },
            new Project
            {
                Id=2,
                BaseSeries="G29856",
            },
        };

        // // CreateProject adds new project to the list
        // public void CreateProject(Project project)
        // {
        //     if (project == null)
        //     {
        //         throw new ArgumentNullException(nameof(project));
        //     }
        //     _projects.Add(project);
        // }

        // GetAllProjects returns the list of projects
        public IEnumerable<Project> GetAllProjects()
        {
            return _projects;
        }

        // GetProjectById returns the project with a given id
        public Project GetProjectById(ulong id)
        {
            try
            {
                var project = _projects[Convert.ToInt32(id)];
                return project;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        // // SaveChanges is a method that is used only in sql repo realization
        // public bool SaveChanges()
        // {
        //     return true;
        // }

        // public void UpdateProject(Project Project)
        // {
        //     // Nothing
        // }
    }
}
