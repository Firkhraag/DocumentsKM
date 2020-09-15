using System.IO;
using DocumentsKM.Data;
using Moq;
using Xunit;

namespace ProjectKM.Tests
{
    public class MarksControllerTest
    {

        // private readonly Mock<IMarkRepo> _repo;

        // [Fact]
        // public async Task Add_to_cart_success()
        // {
        //     //Arrange
        //     var fakeCatalogItem = GetFakeCatalogItem();

        //     _basketServiceMock.Setup(x => x.AddItemToBasket(It.IsAny<ApplicationUser>(), It.IsAny<Int32>()))
        //         .Returns(Task.FromResult(1));

        //     //Act
        //     var orderController = new CartController(_basketServiceMock.Object, _catalogServiceMock.Object, _identityParserMock.Object);
        //     orderController.ControllerContext.HttpContext = _contextMock.Object;
        //     var actionResult = await orderController.AddToCart(fakeCatalogItem);

        //     //Assert
        //     var redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);
        //     Assert.Equal("Catalog", redirectToActionResult.ControllerName);
        //     Assert.Equal("Index", redirectToActionResult.ActionName);
        // }
    }
}