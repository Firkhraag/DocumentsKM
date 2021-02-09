// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DocumentsKM.Data;
// using DocumentsKM.Dtos;
// using DocumentsKM.Models;
// using DocumentsKM.Services;
// using Moq;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     public class SpecificationServiceTest
//     {
//         private readonly Mock<ISpecificationRepo> _mockSpecificationRepo = new Mock<ISpecificationRepo>();
//         private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
//         private readonly ISpecificationService _service;
//         private readonly Random _rnd = new Random();
//         private readonly List<Specification> _specifications = new List<Specification> { };
//         private readonly int _maxMarkId = 3;

//         public SpecificationServiceTest()
//         {
//             // Arrange
//             foreach (var spec in TestData.specifications)
//             {
//                 _specifications.Add(new Specification
//                 {
//                     Id = spec.Id,
//                     Mark = spec.Mark,
//                     IsCurrent = spec.IsCurrent,
//                     Note = spec.Note,
//                     Constructions = new List<Construction> {},
//                     StandardConstructions = new List<StandardConstruction> {},
//                 });
//             }
//             foreach (var specification in _specifications)
//             {
//                 _mockSpecificationRepo.Setup(mock =>
//                     mock.GetById(specification.Id, false)).Returns(
//                         _specifications.SingleOrDefault(v => v.Id == specification.Id));
//                 _mockSpecificationRepo.Setup(mock =>
//                     mock.GetById(specification.Id, true)).Returns(
//                         _specifications.SingleOrDefault(v => v.Id == specification.Id));
//             }
//             foreach (var mark in TestData.marks)
//             {
//                 _mockMarkRepo.Setup(mock =>
//                     mock.GetById(mark.Id)).Returns(
//                         TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

//                 _mockSpecificationRepo.Setup(mock =>
//                     mock.GetAllByMarkId(mark.Id)).Returns(
//                         _specifications.Where(v => v.Mark.Id == mark.Id));
//             }

//             _mockSpecificationRepo.Setup(mock =>
//                 mock.Add(It.IsAny<Specification>())).Verifiable();
//             _mockSpecificationRepo.Setup(mock =>
//                 mock.Update(It.IsAny<Specification>())).Verifiable();
//             _mockSpecificationRepo.Setup(mock =>
//                 mock.Delete(It.IsAny<Specification>())).Verifiable();

//             _service = new SpecificationService(
//                 _mockSpecificationRepo.Object,
//                 _mockMarkRepo.Object);
//         }

//         [Fact]
//         public void GetAllByMarkId_ShouldReturnSpecifications()
//         {
//             // Arrange
//             int markId = _rnd.Next(1, _maxMarkId);

//             // Act
//             var returnedSpecifications = _service.GetAllByMarkId(markId);

//             // Assert
//             Assert.Equal(_specifications.Where(
//                 v => v.Mark.Id == markId), returnedSpecifications);
//         }

//         [Fact]
//         public void Create_ShouldCreateSpecification()
//         {
//             // Arrange
//             int markId = _rnd.Next(1, TestData.marks.Count());

//             // Act
//             var specification = _service.Create(markId);

//             // Assert
//             _mockSpecificationRepo.Verify(
//                 mock => mock.Add(It.IsAny<Specification>()), Times.Once);
//             Assert.NotNull(specification.Mark);

//             int maxNum = 0;
//             foreach (var s in _specifications.Where(v => v.Mark.Id == markId))
//             {
//                 if (s.Num > maxNum)
//                     maxNum = s.Num;
//             }
//             Assert.Equal(maxNum + 1, specification.Num);
//         }

//         [Fact]
//         public void Create_ShouldFailWithNull_WhenWrongMarkId()
//         {
//             // Act & Assert
//             Assert.Throws<ArgumentNullException>(() => _service.Create(999));

//             _mockSpecificationRepo.Verify(
//                 mock => mock.Add(It.IsAny<Specification>()), Times.Never);
//         }

//         [Fact]
//         public void Update_ShouldUpdateSpecification()
//         {
//             // Arrange
//             var id = 2;
//             var spec = _specifications.SingleOrDefault(v => v.Id == id);
//             var newNote = "NewUpdate";
//             var newSpecificationRequest = new SpecificationUpdateRequest
//             {
//                 IsCurrent = true,
//                 Note = newNote,
//             };

//             // Act
//             _service.Update(id, newSpecificationRequest);

//             // Assert
//             _mockSpecificationRepo.Verify(
//                 mock => mock.Update(It.IsAny<Specification>()), Times.Exactly(2));
//             Assert.Equal(newNote, _specifications.SingleOrDefault(v => v.Id == id).Note);
//             Assert.Single(_specifications.Where(
//                 v => v.Mark.Id == spec.Mark.Id && v.IsCurrent));
//         }

//         [Fact]
//         public void Update_ShouldFailWithNull_WhenWrongValues()
//         {
//             // Arrange
//             int id = _rnd.Next(1, _specifications.Count());

//             var newSpecificationRequest = new SpecificationUpdateRequest
//             {
//                 Note = "NewUpdate",
//             };

//             // Act & Assert
//             Assert.Throws<ArgumentNullException>(
//                 () => _service.Update(id, null));
//             Assert.Throws<ArgumentNullException>(
//                 () => _service.Update(999, newSpecificationRequest));

//             _mockSpecificationRepo.Verify(
//                 mock => mock.Update(It.IsAny<Specification>()), Times.Never);
//         }

//         [Fact]
//         public void Delete_ShouldDeleteSpecification()
//         {
//             // Arrange
//             int id = _rnd.Next(1, _specifications.Count());
//             while (_specifications.SingleOrDefault(v => v.Id == id).IsCurrent)
//             {
//                 id = _rnd.Next(1, _specifications.Count());
//             }

//             // Act
//             _service.Delete(id);

//             // Assert
//             _mockSpecificationRepo.Verify(
//                 mock => mock.Delete(It.IsAny<Specification>()), Times.Once);
//         }

//         [Fact]
//         public void Delete_ShouldFailWithNull()
//         {
//             // Act & Assert
//             Assert.Throws<ArgumentNullException>(() => _service.Delete(999));

//             _mockSpecificationRepo.Verify(
//                 mock => mock.Delete(It.IsAny<Specification>()), Times.Never);
//         }

//         [Fact]
//         public void Delete_ShouldFailWithConflict_WhenIsCurrent()
//         {
//             // Arrange
//             int id = _rnd.Next(1, _specifications.Count());
//             while (_specifications.SingleOrDefault(v => v.Id == id).IsCurrent == false)
//             {
//                 id = _rnd.Next(1, _specifications.Count());
//             }

//             // Act & Assert
//             Assert.Throws<ConflictException>(() => _service.Delete(id));

//             _mockSpecificationRepo.Verify(
//                 mock => mock.Delete(It.IsAny<Specification>()), Times.Never);
//         }
//     }
// }
