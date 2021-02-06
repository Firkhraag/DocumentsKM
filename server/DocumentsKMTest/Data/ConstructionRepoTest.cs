using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxSpecificationId = 3;

        private ApplicationContext GetContext(List<Construction> constructions)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ConstructionTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.ConstructionTypes.AddRange(TestData.constructionTypes);
            context.ConstructionSubtypes.AddRange(TestData.constructionSubtypes);
            context.Specifications.AddRange(TestData.specifications);
            context.Constructions.AddRange(constructions);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnConstructions()
        {
            // Arrange
            var context = GetContext(TestData.constructions);
            var repo = new SqlConstructionRepo(context);

            var specificationId = _rnd.Next(1, _maxSpecificationId);

            // Act
            var constructions = repo.GetAllBySpecificationId(specificationId);

            // Assert
            Assert.Equal(TestData.constructions.Where(
                v => v.Specification.Id == specificationId), constructions);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnEmptyArray_WhenWrongSpecificationId()
        {
            // Arrange
            var context = GetContext(TestData.constructions);
            var repo = new SqlConstructionRepo(context);

            // Act
            var constructions = repo.GetAllBySpecificationId(999);

            // Assert
            Assert.Empty(constructions);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnConstruction()
        {
            // Arrange
            var context = GetContext(TestData.constructions);
            var repo = new SqlConstructionRepo(context);

            int id = _rnd.Next(1, TestData.constructions.Count());

            // Act
            var construction = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.constructions.SingleOrDefault(v => v.Id == id), construction);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.constructions);
            var repo = new SqlConstructionRepo(context);

            // Act
            var construction = repo.GetById(999);

            // Assert
            Assert.Null(construction);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnConstruction()
        {
            // Arrange
            var context = GetContext(TestData.constructions);
            var repo = new SqlConstructionRepo(context);

            var specificationId = TestData.constructions[0].Specification.Id;
            var name = TestData.constructions[0].Name;
            var paintworkCoeff = TestData.constructions[0].PaintworkCoeff;

            // Act
            var construction = repo.GetByUniqueKey(
                specificationId, name, paintworkCoeff);

            // Assert
            Assert.Equal(TestData.constructions.SingleOrDefault(
                v => v.Specification.Id == specificationId &&
                    v.Name == name && v.PaintworkCoeff == paintworkCoeff), construction);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
        {
            // Arrange
            var context = GetContext(TestData.constructions);
            var repo = new SqlConstructionRepo(context);

            var specificationId = TestData.constructions[0].Specification.Id;
            var name = TestData.constructions[0].Name;
            var paintworkCoeff = TestData.constructions[0].PaintworkCoeff;

            // Act
            var additionalWork1 = repo.GetByUniqueKey(999, name, paintworkCoeff);
            var additionalWork2 = repo.GetByUniqueKey(specificationId, "NotFound", paintworkCoeff);
            var additionalWork3 = repo.GetByUniqueKey(specificationId, name, -1);

            // Assert
            Assert.Null(additionalWork1);
            Assert.Null(additionalWork2);
            Assert.Null(additionalWork3);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Add_ShouldAddConstruction()
        {
            // Arrange
            var context = GetContext(TestData.constructions);
            var repo = new SqlConstructionRepo(context);

            int specificationId = _rnd.Next(1, TestData.specifications.Count());
            int typeId = _rnd.Next(1, TestData.constructionTypes.Count());
            int subtypeId = _rnd.Next(1, TestData.constructionSubtypes.Count());
            int weldingControlId = _rnd.Next(1, TestData.weldingControl.Count());
            var construction = new Construction
            {
                Specification = TestData.specifications.SingleOrDefault(v => v.Id == specificationId),
                Name = "NewCreate",
                Type = TestData.constructionTypes.SingleOrDefault(v => v.Id == typeId),
                Subtype = TestData.constructionSubtypes.SingleOrDefault(v => v.Id == subtypeId),
                Valuation = "1700",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = false,
                HasDynamicLoad = false,
                HasFlangedConnections = false,
                WeldingControl = TestData.weldingControl.SingleOrDefault(v => v.Id == weldingControlId),
                PaintworkCoeff = 1,
            };

            // Act
            repo.Add(construction);

            // Assert
            Assert.NotNull(repo.GetById(construction.Id));

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Update_ShouldUpdateConstruction()
        {
            // Arrange
            var constructions = new List<Construction> { };
            foreach (var c in TestData.constructions)
            {
                constructions.Add(new Construction
                {
                    Id = c.Id,
                    Specification = c.Specification,
                    Name = c.Name,
                    Type = c.Type,
                    Subtype = c.Subtype,
                    Valuation = c.Valuation,
                    NumOfStandardConstructions = c.NumOfStandardConstructions,
                    StandardAlbumCode = c.StandardAlbumCode,
                    HasEdgeBlunting = c.HasEdgeBlunting,
                    HasDynamicLoad = c.HasDynamicLoad,
                    HasFlangedConnections = c.HasFlangedConnections,
                    WeldingControl = c.WeldingControl,
                    PaintworkCoeff = c.PaintworkCoeff,
                });
            }
            var context = GetContext(constructions);
            var repo = new SqlConstructionRepo(context);

            int id = _rnd.Next(1, constructions.Count());
            var construction = constructions.FirstOrDefault(v => v.Id == id);
            construction.Name = "NewUpdate";

            // Act
            repo.Update(construction);

            // Assert
            Assert.Equal(construction.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Delete_ShouldDeleteConstruction()
        {
            // Arrange
            var context = GetContext(TestData.constructions);
            var repo = new SqlConstructionRepo(context);

            int id = _rnd.Next(1, TestData.constructions.Count());
            var construction = TestData.constructions.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(construction);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
        }
    }
}
