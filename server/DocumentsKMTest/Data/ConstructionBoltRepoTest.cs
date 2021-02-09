// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DocumentsKM.Data;
// using DocumentsKM.Models;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     public class ConstructionBoltRepoTest
//     {
//         private readonly Random _rnd = new Random();
//         private readonly int _maxConstructionId = 3;

//         private ApplicationContext GetContext(List<ConstructionBolt> constructionBolts)
//         {
//             var builder = new DbContextOptionsBuilder<ApplicationContext>();
//             builder.UseInMemoryDatabase(databaseName: "ConstructionBoltTestDb");
//             var options = builder.Options;
//             var context = new ApplicationContext(options);
//             context.Database.EnsureDeleted();
//             context.Database.EnsureCreated();

//             context.BoltDiameters.AddRange(TestData.boltDiameters);
//             context.Constructions.AddRange(TestData.constructions);
//             context.ConstructionBolts.AddRange(constructionBolts);
//             context.SaveChanges();
//             return context;
//         }

//         [Fact]
//         public void GetAllByConstructionId_ShouldReturnConstructionBolts()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionBolts);
//             var repo = new SqlConstructionBoltRepo(context);

//             var constructionId = _rnd.Next(1, _maxConstructionId);

//             // Act
//             var constructionBolts = repo.GetAllByConstructionId(constructionId);

//             // Assert
//             Assert.Equal(TestData.constructionBolts.Where(
//                 v => v.Construction.Id == constructionId), constructionBolts);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetAllByConstructionId_ShouldReturnEmptyArray_WhenWrongConstructionId()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionBolts);
//             var repo = new SqlConstructionBoltRepo(context);

//             // Act
//             var constructionBolts = repo.GetAllByConstructionId(999);

//             // Assert
//             Assert.Empty(constructionBolts);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetById_ShouldReturnConstructionBolt()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionBolts);
//             var repo = new SqlConstructionBoltRepo(context);

//             var id = _rnd.Next(1, TestData.constructionBolts.Count());

//             // Act
//             var constructionBolt = repo.GetById(id);

//             // Assert
//             Assert.Equal(TestData.constructionBolts.SingleOrDefault(v => v.Id == id), constructionBolt);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetById_ShouldReturnNull_WhenWrongId()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionBolts);
//             var repo = new SqlConstructionBoltRepo(context);

//             // Act
//             var constructionBolt = repo.GetById(999);

//             // Assert
//             Assert.Null(constructionBolt);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetByUniqueKey_ShouldReturnConstructionBolt()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionBolts);
//             var repo = new SqlConstructionBoltRepo(context);

//             var constructionId = TestData.constructionBolts[0].Construction.Id;
//             var diameterId = TestData.constructionBolts[0].Diameter.Id;

//             // Act
//             var constructionBolt = repo.GetByUniqueKey(
//                 constructionId, diameterId);

//             // Assert
//             Assert.Equal(TestData.constructionBolts.SingleOrDefault(
//                 v => v.Construction.Id == constructionId &&
//                     v.Diameter.Id == diameterId), constructionBolt);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionBolts);
//             var repo = new SqlConstructionBoltRepo(context);

//             var constructionId = TestData.constructionBolts[0].Construction.Id;
//             var diameterId = TestData.constructionBolts[0].Diameter.Id;

//             // Act
//             var additionalWork1 = repo.GetByUniqueKey(999, diameterId);
//             var additionalWork2 = repo.GetByUniqueKey(constructionId, 999);

//             // Assert
//             Assert.Null(additionalWork1);
//             Assert.Null(additionalWork2);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void Add_ShouldAddConstructionBolt()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionBolts);
//             var repo = new SqlConstructionBoltRepo(context);

//             int constructionId = _rnd.Next(1, TestData.marks.Count());
//             int diameterId = _rnd.Next(1, TestData.boltDiameters.Count());
//             var constructionBolt = new ConstructionBolt
//             {
//                 Construction = TestData.constructions.SingleOrDefault(
//                     v => v.Id == constructionId),
//                 Diameter = TestData.boltDiameters.SingleOrDefault(
//                     v => v.Id == diameterId),
//                 Packet = 5,
//                 Num = 5,
//                 NutNum = 5,
//                 WasherNum = 5,
//             };

//             // Act
//             repo.Add(constructionBolt);

//             // Assert
//             Assert.NotNull(repo.GetById(constructionBolt.Id));

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void Update_ShouldUpdateConstructionBolt()
//         {
//             // Arrange
//             var constructionBolts = new List<ConstructionBolt> { };
//             foreach (var cb in TestData.constructionBolts)
//             {
//                 constructionBolts.Add(new ConstructionBolt
//                 {
//                     Id = cb.Id,
//                     Construction = cb.Construction,
//                     Diameter = cb.Diameter,
//                     Packet = cb.Packet,
//                     Num = cb.Num,
//                     NutNum = cb.NutNum,
//                     WasherNum = cb.WasherNum,
//                 });
//             }
//             var context = GetContext(constructionBolts);
//             var repo = new SqlConstructionBoltRepo(context);

//             int id = _rnd.Next(1, constructionBolts.Count());
//             var constructionBolt = constructionBolts.FirstOrDefault(v => v.Id == id);
//             constructionBolt.NutNum = 99;

//             // Act
//             repo.Update(constructionBolt);

//             // Assert
//             Assert.Equal(constructionBolt.NutNum, repo.GetById(id).NutNum);

//             context.Database.EnsureDeleted();
//         }

//         [Fact]
//         public void Delete_ShouldDeleteConstructionBolt()
//         {
//             // Arrange
//             var context = GetContext(TestData.constructionBolts);
//             var repo = new SqlConstructionBoltRepo(context);

//             int id = _rnd.Next(1, TestData.constructionBolts.Count());
//             var constructionBolt = TestData.constructionBolts.FirstOrDefault(
//                 v => v.Id == id);

//             // Act
//             repo.Delete(constructionBolt);

//             // Assert
//             Assert.Null(repo.GetById(id));

//             context.Database.EnsureDeleted();
//         }
//     }
// }
