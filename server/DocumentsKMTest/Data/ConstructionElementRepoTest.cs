// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DocumentsKM.Data;
// using DocumentsKM.Models;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     public class ConstructionElementRepoTest
//     {
//         private readonly Random _rnd = new Random();
//         private readonly int _maxConstructionId = 3;

//         private ApplicationContext GetContext(List<ConstructionElement> constructionElements)
//         {
//             var builder = new DbContextOptionsBuilder<ApplicationContext>();
//             builder.UseInMemoryDatabase(databaseName: "ConstructionElementTestDb");
//             var options = builder.Options;
//             var context = new ApplicationContext(options);
//             context.Database.EnsureDeleted();
//             context.Database.EnsureCreated();

//             context.ProfileClasses.AddRange(TestData.profileClasses);
//             context.Profiles.AddRange(TestData.profiles);
//             context.Steel.AddRange(TestData.steel);
//             context.Constructions.AddRange(TestData.constructions);
//             context.ConstructionElements.AddRange(constructionElements);
//             context.SaveChanges();
//             return context;
//         }

//         [Fact]
//         public void GetAllByConstructionId_ShouldReturnConstructionElements()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionElements);
//             var repo = new SqlConstructionElementRepo(context);

//             var constructionId = _rnd.Next(1, _maxConstructionId);

//             // Act
//             var constructionElements = repo.GetAllByConstructionId(constructionId);

//             // Assert
//             Assert.Equal(TestData.constructionElements.Where(
//                 v => v.Construction.Id == constructionId), constructionElements);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetAllByConstructionId_ShouldReturnEmptyArray_WhenWrongConstructionId()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionElements);
//             var repo = new SqlConstructionElementRepo(context);

//             // Act
//             var constructionElements = repo.GetAllByConstructionId(999);

//             // Assert
//             Assert.Empty(constructionElements);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetById_ShouldReturnConstructionElement()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionElements);
//             var repo = new SqlConstructionElementRepo(context);

//             var id = _rnd.Next(1, TestData.constructionElements.Count());

//             // Act
//             var constructionElement = repo.GetById(id);

//             // Assert
//             Assert.Equal(TestData.constructionElements.SingleOrDefault(v => v.Id == id), constructionElement);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetById_ShouldReturnNull_WhenWrongId()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionElements);
//             var repo = new SqlConstructionElementRepo(context);

//             // Act
//             var constructionElement = repo.GetById(999);

//             // Assert
//             Assert.Null(constructionElement);

//             context.Database.EnsureDeleted();
//         }

//         // [Fact]
//         // public void Add_ShouldAddConstructionElement()
//         // {
//         //     // Arrange
//         //     var context = GetContext(TestData.constructionElements);
//         //     var repo = new SqlConstructionElementRepo(context);

//         //     int constructionId = _rnd.Next(1, TestData.marks.Count());
//         //     int profileClassId = _rnd.Next(1, TestData.profileClasses.Count());
//         //     int profileId = _rnd.Next(1, TestData.profiles.Count());
//         //     int steelId = _rnd.Next(1, TestData.steel.Count());
//         //     var constructionElement = new ConstructionElement
//         //     {
//         //         Construction = TestData.constructions.SingleOrDefault(
//         //             v => v.Id == constructionId),
//         //         ProfileClass = TestData.profileClasses.SingleOrDefault(
//         //             v => v.Id == profileClassId),
//         //         Profile = TestData.profiles.SingleOrDefault(
//         //             v => v.Id == profileId),
//         //         Steel = TestData.steel.SingleOrDefault(
//         //             v => v.Id == steelId),
//         //         Length = 1.0f,
//         //     };

//         //     // Act
//         //     repo.Add(constructionElement);

//         //     // Assert
//         //     Assert.NotNull(repo.GetById(constructionElement.Id));

//         //     context.Database.EnsureDeleted();
//         // }

//         // [Fact]
//         // public void Update_ShouldUpdateConstructionElement()
//         // {
//         //     // Arrange
//         //     var constructionElements = new List<ConstructionElement> { };
//         //     foreach (var ce in TestData.constructionElements)
//         //     {
//         //         constructionElements.Add(new ConstructionElement
//         //         {
//         //             Id = ce.Id,
//         //             Construction = ce.Construction,
//         //             ProfileClass = ce.ProfileClass,
//         //             Profile = ce.Profile,
//         //             Steel = ce.Steel,
//         //             Length = ce.Length,
//         //         });
//         //     }
//         //     var context = GetContext(constructionElements);
//         //     var repo = new SqlConstructionElementRepo(context);

//         //     int id = _rnd.Next(1, constructionElements.Count());
//         //     var constructionElement = constructionElements.FirstOrDefault(v => v.Id == id);
//         //     constructionElement.Length = 9;

//         //     // Act
//         //     repo.Update(constructionElement);

//         //     // Assert
//         //     Assert.Equal(constructionElement.Length, repo.GetById(id).Length);

//         //     context.Database.EnsureDeleted();
//         // }

//         [Fact]
//         public void Delete_ShouldDeleteConstructionElement()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionElements);
//             var repo = new SqlConstructionElementRepo(context);

//             int id = _rnd.Next(1, TestData.constructionElements.Count());
//             var constructionElement = TestData.constructionElements.FirstOrDefault(
//                 v => v.Id == id);

//             // Act
//             repo.Delete(constructionElement);

//             // Assert
//             Assert.Null(repo.GetById(id));

//             context.Database.EnsureDeleted();
//         }
//     }
// }
