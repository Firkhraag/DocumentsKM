using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarkRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<Mark> marks)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "MarkTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Subnodes.AddRange(TestData.subnodes);
            context.Marks.AddRange(marks);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnMarks()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            // Act
            var marks = repo.GetAll();

            // Assert
            Assert.Equal(TestData.marks, marks);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllBySubnodeId_ShouldReturnMarks()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            var subnodeId = _rnd.Next(1, TestData.subnodes.Count());

            // Act
            var marks = repo.GetAllBySubnodeId(subnodeId);

            // Assert
            Assert.Equal(TestData.marks.Where(v => v.Subnode.Id == subnodeId), marks);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllBySubnodeId_ShouldReturnEmptyArray_WhenWrongSubnodeId()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            // Act
            var marks = repo.GetAllBySubnodeId(999);

            // Assert
            Assert.Empty(marks);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnMark()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            int id = _rnd.Next(1, TestData.marks.Count());

            // Act
            var mark = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.marks.SingleOrDefault(v => v.Id == id),
                mark);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Act
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            var mark = repo.GetById(999);

            // Assert
            Assert.Null(mark);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnMark()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            var id = 1;
            var subnodeId = TestData.marks[0].Subnode.Id;
            var code = TestData.marks[0].Code;

            // Act
            var mark = repo.GetByUniqueKey(subnodeId, code);

            // Assert
            Assert.Equal(id, mark.Id);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull_WhenWrongSubnodeIdOrCode()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            var subnodeId = TestData.marks[0].Subnode.Id;
            var code = TestData.marks[0].Code;

            // Act
            var mark1 = repo.GetByUniqueKey(999, code);
            var mark2 = repo.GetByUniqueKey(subnodeId, "NotFound");

            // Assert
            Assert.Null(mark1);
            Assert.Null(mark2);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddMark()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            int subnodeId = _rnd.Next(1, TestData.subnodes.Count());
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            int chiefSpecialistId = _rnd.Next(1, TestData.employees.Count());
            int groupLeaderId = _rnd.Next(1, TestData.employees.Count());
            int mainBuilderId = _rnd.Next(1, TestData.employees.Count());
            var mark = new Mark
            {
                Subnode = TestData.subnodes.SingleOrDefault(
                    v => v.Id == subnodeId
                ),
                Code = "NewCreate",
                Name = "NewCreate",
                Department = TestData.departments.SingleOrDefault(
                    v => v.Id == departmentId
                ),
                ChiefSpecialist = TestData.employees.SingleOrDefault(
                    v => v.Id == chiefSpecialistId
                ),
                GroupLeader = TestData.employees.SingleOrDefault(
                    v => v.Id == groupLeaderId
                ),
                MainBuilder = TestData.employees.SingleOrDefault(
                    v => v.Id == mainBuilderId
                ),
            };

            // Act
            repo.Add(mark);

            // Assert
            Assert.NotNull(repo.GetById(mark.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateMark()
        {
            // Arrange
            var marks = new List<Mark>{};
            foreach (var m in TestData.marks)
            {
                marks.Add(new Mark
                {
                    Id = m.Id,
                    Subnode = m.Subnode,
                    Code = m.Code,
                    Name = m.Name,
                    Department = m.Department,
                    ChiefSpecialist = m.ChiefSpecialist,
                    GroupLeader = m.GroupLeader,
                    MainBuilder = m.MainBuilder,
                    EditedDate = m.EditedDate,
                    Signed1Id = m.Signed1Id,
                    Signed2Id = m.Signed2Id,
                    IssueDate = m.IssueDate,
                    NumOfVolumes = m.NumOfVolumes,
                    PaintworkType = m.PaintworkType,
                    Note = m.Note,
                    FireHazardCategoryId = m.FireHazardCategoryId,
                    PTransport = m.PTransport,
                    PSite = m.PSite,
                });
            }
            var context = GetContext(marks);
            var repo = new SqlMarkRepo(context);

            int id = _rnd.Next(1, marks.Count());
            var mark = marks.FirstOrDefault(v => v.Id == id);
            mark.Name = "NewUpdate";

            // Act
            repo.Update(mark);

            // Assert
            Assert.Equal(mark.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
