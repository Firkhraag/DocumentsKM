// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DocumentsKM.Data;
// using DocumentsKM.Models;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     public class StandardConstructionRepoTest
//     {
//         private readonly Random _rnd = new Random();
//         private readonly int _maxSpecificationId = 3;

//         private ApplicationContext GetContext(List<StandardConstruction> standardConstructions)
//         {
//             var builder = new DbContextOptionsBuilder<ApplicationContext>();
//             builder.UseInMemoryDatabase(databaseName: "StandardConstructionTestDb");
//             var options = builder.Options;
//             var context = new ApplicationContext(options);
//             context.Database.EnsureDeleted();
//             context.Database.EnsureCreated();

//             // context.Specifications.AddRange(TestData.specifications);

//             // foreach (var sc in standardConstructions)
//             // {
//             //     if (context.StandardConstructions.SingleOrDefault(v => v.Id == sc.Id) == null)
//             //     {
//             //         context.StandardConstructions.Add(sc);
//             //     }
//             // }

//             context.StandardConstructions.AddRange(standardConstructions);
            
//             context.SaveChanges();
//             return context;
//         }

//         [Fact]
//         public void GetAllBySpecificationId_ShouldReturnStandardConstructions()
//         {
//             // Arrange
//             var context = GetContext(TestData.standardConstructions);
//             var repo = new SqlStandardConstructionRepo(context);

//             var specificationId = _rnd.Next(1, _maxSpecificationId);

//             // Act
//             var standardConstructions = repo.GetAllBySpecificationId(specificationId);

//             // Assert
//             Assert.Equal(TestData.standardConstructions.Where(
//                 v => v.Specification.Id == specificationId), standardConstructions);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetAllBySpecificationId_ShouldReturnEmptyArray_WhenWrongSpecificationId()
//         {
//             // Arrange
//             var context = GetContext(TestData.standardConstructions);
//             var repo = new SqlStandardConstructionRepo(context);

//             // Act
//             var standardConstructions = repo.GetAllBySpecificationId(999);

//             // Assert
//             Assert.Empty(standardConstructions);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetById_ShouldReturnStandardConstruction()
//         {
//             // Arrange
//             var context = GetContext(TestData.standardConstructions);
//             var repo = new SqlStandardConstructionRepo(context);

//             int id = _rnd.Next(1, TestData.standardConstructions.Count());

//             // Act
//             var standardConstruction = repo.GetById(id);

//             // Assert
//             Assert.Equal(TestData.standardConstructions.SingleOrDefault(
//                 v => v.Id == id), standardConstruction);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetById_ShouldReturnNull_WhenWrongId()
//         {
//             // Arrange
//             var context = GetContext(TestData.standardConstructions);
//             var repo = new SqlStandardConstructionRepo(context);

//             // Act
//             var standardConstruction = repo.GetById(999);

//             // Assert
//             Assert.Null(standardConstruction);

//             context.Database.EnsureDeleted();
//         }

//         // [Fact]
//         // public void GetByUniqueKey_ShouldReturnStandardConstruction()
//         // {
//         //     // Arrange
//         //     var context = GetContext(TestData.standardConstructions);
//         //     var repo = new SqlStandardConstructionRepo(context);

//         //     var specificationId = TestData.StandardConstructions[0].Specification.Id;
//         //     var name = TestData.StandardConstructions[0].Name;
//         //     var paintworkCoeff = TestData.StandardConstructions[0].PaintworkCoeff;

//         //     // Act
//         //     var StandardConstruction = repo.GetByUniqueKey(
//         //         specificationId, name, paintworkCoeff);

//         //     // Assert
//         //     Assert.Equal(TestData.StandardConstructions.SingleOrDefault(
//         //         v => v.Specification.Id == specificationId &&
//         //             v.Name == name && v.PaintworkCoeff == paintworkCoeff), StandardConstruction);

//         //     context.Database.EnsureDeleted();
//         // }

//         // [Fact]
//         // public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
//         // {
//         //     // Arrange
//         //     var context = GetContext(TestData.standardConstructions);
//         //     var repo = new SqlStandardConstructionRepo(context);

//         //     var specificationId = TestData.StandardConstructions[0].Specification.Id;
//         //     var wrongSpecificationId = 999;
//         //     var name = TestData.StandardConstructions[0].Name;
//         //     var wrongName = "wrong";
//         //     var paintworkCoeff = TestData.StandardConstructions[0].PaintworkCoeff;
//         //     var wrongPaintworkCoeff = -1;

//         //     // Act
//         //     var additionalWork1 = repo.GetByUniqueKey(wrongSpecificationId, name, paintworkCoeff);
//         //     var additionalWork2 = repo.GetByUniqueKey(specificationId, wrongName, paintworkCoeff);
//         //     var additionalWork3 = repo.GetByUniqueKey(specificationId, name, wrongPaintworkCoeff);

//         //     // Assert
//         //     Assert.Null(additionalWork1);
//         //     Assert.Null(additionalWork2);
//         //     Assert.Null(additionalWork3);

//         //     context.Database.EnsureDeleted();
//         // }

//         // [Fact]
//         // public void Add_ShouldAddStandardConstruction()
//         // {
//         //     // Arrange
//         //     var context = GetContext(TestData.standardConstructions);
//         //     var repo = new SqlStandardConstructionRepo(context);

//         //     int specificationId = _rnd.Next(1, TestData.specifications.Count());
//         //     var standardConstruction = new StandardConstruction
//         //     {
//         //         Specification = TestData.specifications.SingleOrDefault(v => v.Id == specificationId),
//         //         Name = "NewCreate",
//         //         Num = 1,
//         //         Sheet = "NewCreate",
//         //         Weight = 1.0f,
//         //     };

//         //     // Act
//         //     repo.Add(standardConstruction);

//         //     // Assert
//         //     Assert.NotNull(repo.GetById(standardConstruction.Id));

//         //     context.Database.EnsureDeleted();
//         // }

//         // [Fact]
//         // public void Update_ShouldUpdateStandardConstruction()
//         // {
//         //     // Arrange
//         //     var standardConstructions = new List<StandardConstruction> { };
//         //     foreach (var c in TestData.standardConstructions)
//         //     {
//         //         standardConstructions.Add(new StandardConstruction
//         //         {
//         //             Id = c.Id,
//         //             Specification = c.Specification,
//         //             Name = c.Name,
//         //             Num = c.Num,
//         //             Sheet = c.Sheet,
//         //             Weight = c.Weight,
//         //         });
//         //     }
//         //     var context = GetContext(standardConstructions);
//         //     var repo = new SqlStandardConstructionRepo(context);

//         //     int id = _rnd.Next(1, standardConstructions.Count());
//         //     var standardConstruction = standardConstructions.FirstOrDefault(
//         //         v => v.Id == id);
//         //     standardConstruction.Name = "NewUpdate";

//         //     // Act
//         //     repo.Update(standardConstruction);

//         //     // Assert
//         //     Assert.Equal(standardConstruction.Name, repo.GetById(id).Name);

//         //     context.Database.EnsureDeleted();
//         // }

//         [Fact]
//         public void Delete_ShouldDeleteStandardConstruction()
//         {
//             // Arrange
//             var context = GetContext(TestData.standardConstructions);
//             var repo = new SqlStandardConstructionRepo(context);

//             int id = _rnd.Next(1, TestData.standardConstructions.Count());
//             var standardConstruction = TestData.standardConstructions.FirstOrDefault(
//                 v => v.Id == id);

//             // Act
//             repo.Delete(standardConstruction);

//             // Assert
//             Assert.Null(repo.GetById(id));

//             context.Database.EnsureDeleted();
//         }
//     }
// }
