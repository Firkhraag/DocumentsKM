using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DepartmentServiceTest
    {
        private readonly IDepartmentService _service;

        public DepartmentServiceTest()
        {
            // Arrange
            var repository = new Mock<IDepartmentRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.departments.Where(v => v.IsActive));

            _service = new DepartmentService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnDepartments()
        {
            // Act
            var returnedDepartments = _service.GetAll();

            // Assert
            Assert.Equal(TestData.departments.Where(v => v.IsActive), returnedDepartments);
        }
    }
}
