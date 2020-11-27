using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SpecificationServiceTest
    {
        private readonly Mock<ISpecificationRepo> _mockSpecificationRepo = new Mock<ISpecificationRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly SpecificationService _service;
        private readonly Random _rnd = new Random();

        public SpecificationServiceTest()
        {
            // Arrange
            foreach (var specification in TestData.specifications)
            {
                _mockSpecificationRepo.Setup(mock=>
                    mock.GetById(specification.Id)).Returns(
                        TestData.specifications.SingleOrDefault(v => v.Id == specification.Id));
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock=>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _mockSpecificationRepo.Setup(mock=>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        TestData.specifications.Where(v => v.Mark.Id == mark.Id));
            }

            _mockSpecificationRepo.Setup(mock=>
                mock.Add(It.IsAny<Specification>())).Verifiable();
            _mockSpecificationRepo.Setup(mock=>
                mock.Update(It.IsAny<Specification>())).Verifiable();
            _mockSpecificationRepo.Setup(mock=>
                mock.Delete(It.IsAny<Specification>())).Verifiable();

            _service = new SpecificationService(
                _mockSpecificationRepo.Object,
                _mockMarkRepo.Object);
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnAllSpecificationsWithGivenMarkId()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            
            // Act
            var returnedSpecifications = _service.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.specifications.Where(
                v => v.Mark.Id == markId), returnedSpecifications);
        }

        [Fact]
        public void Create_ShouldCreateSpecification()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            
            // Act
            var specification = _service.Create(markId);

            // Assert
            _mockSpecificationRepo.Verify(mock => mock.Add(It.IsAny<Specification>()), Times.Once);
            Assert.NotNull(specification.Mark);

            int maxNum = 0;
            foreach (var s in TestData.specifications.Where(v => v.Mark.Id == markId))
            {
                if (s.Num > maxNum)
                    maxNum = s.Num;
            }
            Assert.Equal(maxNum + 1, specification.Num);
        }

        [Fact]
        public void Create_ShouldFailWithNull()
        {
            // Arrange
            int wrongMarkId = 999;
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(wrongMarkId));

            _mockSpecificationRepo.Verify(mock => mock.Add(It.IsAny<Specification>()), Times.Never);
        }

        // TBD!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        
        // [Fact]
        // public void Update_ShouldUpdateSpecification()
        // {
        //     // Arrange
        //     int id = _rnd.Next(1, TestData.specifications.Count());
        //     var newNote = "NewUpdate";

        //     var newSpecificationRequest = new SpecificationUpdateRequest{
        //         Designation=newDesignation,
        //     };
            
        //     // Act
        //     _service.Update(id, newSpecificationRequest);

        //     // Assert
        //     _mockSpecificationRepo.Verify(mock => mock.Update(It.IsAny<Specification>()), Times.Once);
        //     Assert.Equal(newDesignation, TestData.Specifications.SingleOrDefault(v => v.Id == id).Designation);
        // }

        // [Fact]
        // public void Update_ShouldFailWithNull()
        // {
        //     // Arrange
        //     int id = _rnd.Next(1, TestData.Specifications.Count());
        //     int wrongId = 999;

        //     var newSpecificationRequest = new SpecificationUpdateRequest{
        //         Designation="NewUpdate",
        //         Name="NewUpdate",
        //     };
            
        //     // Act & Assert
        //     Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
        //     Assert.Throws<ArgumentNullException>(() => _service.Update(wrongId, newSpecificationRequest));

        //     _mockSpecificationRepo.Verify(mock => mock.Update(It.IsAny<Specification>()), Times.Never);
        // }

        // [Fact]
        // public void Update_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     // Possible conflict values
        //     var conflictMarkId = TestData.Specifications[0].Mark.Id;
        //     var conflictDesignation = TestData.Specifications[0].Designation;
        //     var id = TestData.Specifications[3].Id;

        //     var newSpecificationRequest = new SpecificationUpdateRequest{
        //         Designation=conflictDesignation,
        //         Name="NewUpdate",
        //     };
            
        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Update(id, newSpecificationRequest));

        //     _mockSpecificationRepo.Verify(mock => mock.Update(It.IsAny<Specification>()), Times.Never);
        // }

        [Fact]
        public void Delete_ShouldDeleteSpecification()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.specifications.Count());
            while (TestData.specifications.SingleOrDefault(v => v.Id == id).IsCurrent)
            {
                id = _rnd.Next(1, TestData.specifications.Count());
            }
            
            // Act
            _service.Delete(id);

            // Assert
            _mockSpecificationRepo.Verify(mock => mock.Delete(It.IsAny<Specification>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull()
        {
            // Arrange
            var wrongId = 999;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(wrongId));

            _mockSpecificationRepo.Verify(mock => mock.Delete(It.IsAny<Specification>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldFailWithConflict()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.specifications.Count());
            while (TestData.specifications.SingleOrDefault(v => v.Id == id).IsCurrent == false)
            {
                id = _rnd.Next(1, TestData.specifications.Count());
            }

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Delete(id));

            _mockSpecificationRepo.Verify(mock => mock.Delete(It.IsAny<Specification>()), Times.Never);
        }
    }
}











// using System.Collections.Generic;
// using DocumentsKM.Models;
// using DocumentsKM.Data;
// using System;
// using DocumentsKM.Dtos;

// namespace DocumentsKM.Services
// {
//     public class SpecificationService : ISpecificationService
//     {
//         private ISpecificationRepo _repository;
//         private readonly IMarkRepo _markRepo;

//         public SpecificationService(
//             ISpecificationRepo specificationRepo,
//             IMarkRepo markRepo)
//         {
//             _repository = specificationRepo;
//             _markRepo = markRepo;
//         }

//         public IEnumerable<Specification> GetAllByMarkId(int markId)
//         {
//             return _repository.GetAllByMarkId(markId);
//         }

//         public Specification Create(int markId)
//         {
//             var foundMark = _markRepo.GetById(markId);
//             if (foundMark == null)
//                 throw new ArgumentNullException(nameof(foundMark));
//             var specifications = _repository.GetAllByMarkId(markId);
//             int maxNum = 0;
//             foreach (var s in specifications)
//             {
//                 if (s.IsCurrent)
//                 {
//                     s.IsCurrent = false;
//                     _repository.Update(s);
//                 }
//                 if (s.Num > maxNum)
//                     maxNum = s.Num;
//             }
                
//             var newSpecification = new Specification{
//                 Mark = foundMark,
//                 Num = maxNum + 1,
//                 IsCurrent = true,
//             };
//             _repository.Add(newSpecification);
//             return newSpecification;
//         }

//         public void Update(
//             int id,
//             SpecificationUpdateRequest specification)
//         {
//             if (specification == null)
//                 throw new ArgumentNullException(nameof(specification));
//             var foundSpecification = _repository.GetById(id);
//             if (foundSpecification == null)
//                 throw new ArgumentNullException(nameof(foundSpecification));

//             if (specification.IsCurrent != null)
//                 foundSpecification.IsCurrent = specification.IsCurrent ?? false;
//             if (specification.Note != null)
//                 foundSpecification.Note = specification.Note;
//             _repository.Update(foundSpecification);
//         }

//         public void Delete(int id)
//         {
//             var foundSpecification = _repository.GetById(id);
//             if (foundSpecification == null)
//                 throw new ArgumentNullException(nameof(foundSpecification));
//             if (foundSpecification.IsCurrent)
//                 throw new ConflictException(nameof(foundSpecification.IsCurrent));

//             _repository.Delete(foundSpecification);
//         }
//     }
// }
