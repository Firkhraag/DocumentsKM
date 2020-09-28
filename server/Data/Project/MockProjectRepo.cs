using System;
using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    // Мок репозитория для тестирования
    public class MockProjectRepo : IProjectRepo
    {
        // Начальные значения
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

        public IEnumerable<Project> GetAll()
        {
            return _projects;
        }

        public Project GetById(int id)
        {
            try
            {
                return _projects[id];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}
