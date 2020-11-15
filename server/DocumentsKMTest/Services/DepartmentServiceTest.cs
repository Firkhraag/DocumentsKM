using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DepartmentServiceTest
    {
        [Fact]
        public void GetAll_ShouldReturnAllDepartments()
        {
            // Arrange
            var departments = TestData.departments;
            var mockDepartmentRepo = new Mock<IDepartmentRepo>();
            mockDepartmentRepo.Setup(mock=>
                mock.GetAll()).Returns(departments);
            var service = new DepartmentService(mockDepartmentRepo.Object);
            
            // Act
            var returnedDepartments = service.GetAll();

            // Assert
            Assert.Equal(departments, returnedDepartments);
        }
    }
}
