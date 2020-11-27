using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class LinkedDocServiceTest
    {
        private readonly LinkedDocService _service;
        private readonly Random _rnd = new Random();

        public LinkedDocServiceTest()
        {
            // Arrange
            var mockLinkedDocRepo = new Mock<ILinkedDocRepo>();

            foreach (var type in TestData.linkedDocTypes)
            {
                mockLinkedDocRepo.Setup(mock=>
                    mock.GetAllByDocTypeId(type.Id)).Returns(
                        TestData.linkedDocs.Where(v => v.Type.Id == type.Id));
            }

            _service = new LinkedDocService(mockLinkedDocRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllLinkedDocs()
        {
            // Arrange
            int typeId = _rnd.Next(1, TestData.linkedDocTypes.Count());

            // Act
            var returnedLinkedDocs = _service.GetAllByDocTypeId(typeId);

            // Assert
            Assert.Equal(TestData.linkedDocs.Where(v => v.Type.Id == typeId),
                returnedLinkedDocs);
        }
    }
}
