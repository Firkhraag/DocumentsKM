using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DepartmentServiceTest
    {
        private readonly DepartmentService _service;

        public DepartmentServiceTest()
        {
            // Arrange
            var mockDepartmentRepo = new Mock<IDepartmentRepo>();

            mockDepartmentRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.departments);
                
            _service = new DepartmentService(mockDepartmentRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllDepartments()
        {
            // Act
            var returnedDepartments = _service.GetAll();

            // Assert
            Assert.Equal(TestData.departments, returnedDepartments);
        }
    }
}
