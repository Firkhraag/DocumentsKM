// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DocumentsKM.Data;
// using DocumentsKM.Models;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     public class ProjectRepoTest
//     {
//         private readonly Random _rnd = new Random();

//         private ApplicationContext GetContext(List<Project> projects)
//         {
//             var builder = new DbContextOptionsBuilder<ApplicationContext>();
//             builder.UseInMemoryDatabase(databaseName: "ProjectTestDb");
//             var options = builder.Options;
//             var context = new ApplicationContext(options);
//             context.Database.EnsureDeleted();
//             context.Database.EnsureCreated();
//             context.Projects.AddRange(projects);
//             context.SaveChanges();
//             return context;
//         }

//         [Fact]
//         public void GetAll_ShouldReturnProjects()
//         {
//             // Arrange
//             var context = GetContext(TestData.projects);
//             var repo = new SqlProjectRepo(context);

//             // Act
//             var projects = repo.GetAll();

//             // Assert
//             Assert.Equal(TestData.projects, projects);

//             context.Database.EnsureDeleted();
//             context.Dispose();
//         }

//         [Fact]
//         public void GetById_ShouldReturnProject()
//         {
//             // Arrange
//             var context = GetContext(TestData.projects);
//             var repo = new SqlProjectRepo(context);

//             int id = _rnd.Next(1, TestData.projects.Count());

//             // Act
//             var project = repo.GetById(id);

//             // Assert
//             Assert.Equal(TestData.projects.SingleOrDefault(v => v.Id == id), project);

//             context.Database.EnsureDeleted();
//             context.Dispose();
//         }

//         [Fact]
//         public void GetById_ShouldReturnNull_WhenWrongId()
//         {
//             // Arrange
//             var context = GetContext(TestData.projects);
//             var repo = new SqlProjectRepo(context);

//             // Act
//             var project = repo.GetById(999);

//             // Assert
//             Assert.Null(project);

//             context.Database.EnsureDeleted();
//             context.Dispose();
//         }
//     }
// }
