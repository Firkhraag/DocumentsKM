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
        }

        [Fact]
        public void GetAllBySubnodeId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            var wrongSubnodeId = 999;

            // Act
            var marks = repo.GetAllBySubnodeId(wrongSubnodeId);

            // Assert
            Assert.Empty(marks);

            context.Database.EnsureDeleted();
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
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Act
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            var mark = repo.GetById(999);

            // Assert
            Assert.Null(mark);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetBySubnodeIdAndCode_ShouldReturnMark()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            var id = 1;
            var subnodeId = TestData.marks[0].Subnode.Id;
            var code = TestData.marks[0].Code;

            // Act
            var mark = repo.GetBySubnodeIdAndCode(subnodeId, code);

            // Assert
            Assert.Equal(id, mark.Id);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetBySubnodeIdAndCode_ShouldReturnNull_WhenWrongSubnodeId()
        {
            // Arrange
            var context = GetContext(TestData.marks);
            var repo = new SqlMarkRepo(context);

            var subnodeId = TestData.marks[0].Subnode.Id;
            var wrongSubnodeId = 999;
            var code = TestData.marks[0].Code;
            var wrongCode = "NotFound";

            // Act
            var mark1 = repo.GetBySubnodeIdAndCode(wrongSubnodeId, code);
            var mark2 = repo.GetBySubnodeIdAndCode(subnodeId, wrongCode);

            // Assert
            Assert.Null(mark1);
            Assert.Null(mark2);

            context.Database.EnsureDeleted();
        }

        // [Fact]
        // public void Add_ShouldAddMark()
        // {
        //     // Arrange
        //     var context = GetContext(TestData.Marks);
        //     var repo = new SqlMarkRepo(context);

        //     int markId = _rnd.Next(1, TestData.marks.Count());
        //     var Mark = new Mark
        //     {
        //         Mark=TestData.marks.SingleOrDefault(v => v.Id == markId),
        //         Designation="NewCreate",
        //         Name="NewCreate",
        //     };

        //     // Act
        //     repo.Add(Mark);

        //     // Assert
        //     Assert.NotEqual(0, Mark.Id);
        //     Assert.Equal(
        //         TestData.Marks.Where(v => v.Mark.Id == markId).Count() + 1,
        //         repo.GetAllByMarkId(markId).Count());

        //     context.Database.EnsureDeleted();
        // }

        // [Fact]
        // public void Update_ShouldUpdateMark()
        // {
        //     // Arrange
        //     var Marks = new List<Mark>{};
        //     foreach (var ad in TestData.Marks)
        //     {
        //         Marks.Add(new Mark
        //         {
        //             Id = ad.Id,
        //             Mark = ad.Mark,
        //             Designation = ad.Designation,
        //             Name = ad.Name,
        //         });
        //     }
        //     var context = GetContext(Marks);
        //     var repo = new SqlMarkRepo(context);

        //     int id = _rnd.Next(1, Marks.Count());
        //     var Mark = Marks.FirstOrDefault(v => v.Id == id);
        //     Mark.Name = "NewUpdate";

        //     // Act
        //     repo.Update(Mark);

        //     // Assert
        //     Assert.Equal(Mark.Name, repo.GetById(id).Name);

        //     context.Database.EnsureDeleted();
        // }
    }
}
