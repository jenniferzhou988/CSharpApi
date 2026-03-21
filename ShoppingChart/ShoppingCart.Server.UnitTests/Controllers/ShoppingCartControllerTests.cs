using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingCartAPI.Controllers;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Repository.Interface;
using Xunit;

namespace ShoppingCart.Server.UnitTests.Controllers;

public class ShoppingCartControllerTests
{
    private readonly Mock<IShoppingCartRepository> _mockRepo;
    private readonly ShoppingCartController _controller;

    public ShoppingCartControllerTests()
    {
        _mockRepo = new Mock<IShoppingCartRepository>();
        _controller = new ShoppingCartController(_mockRepo.Object);
    }

    #region GetAll

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfCarts()
    {
        // Arrange
        var carts = new List<ShoppingCartAPI.Models.ShoppingCart>
        {
            new() { Id = 1, CustomerId = 10, Status = 1, Created = DateTime.UtcNow },
            new() { Id = 2, CustomerId = 20, Status = 1, Created = DateTime.UtcNow }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(carts);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCarts = Assert.IsAssignableFrom<IEnumerable<ShoppingCartAPI.Models.ShoppingCart>>(okResult.Value);
        Assert.Equal(2, returnedCarts.Count());
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(new List<ShoppingCartAPI.Models.ShoppingCart>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCarts = Assert.IsAssignableFrom<IEnumerable<ShoppingCartAPI.Models.ShoppingCart>>(okResult.Value);
        Assert.Empty(returnedCarts);
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetById_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var cart = new ShoppingCartAPI.Models.ShoppingCart
        {
            Id = 1,
            CustomerId = 10,
            Status = 1
        };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cart);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCart = Assert.IsType<ShoppingCartAPI.Models.ShoppingCart>(okResult.Value);
        Assert.Equal(1, returnedCart.Id);
        Assert.Equal(10, returnedCart.CustomerId);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(999))
                 .ReturnsAsync((ShoppingCartAPI.Models.ShoppingCart?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_ValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var request = new CreateShoppingCartRequest { CustomerId = 10 };
        var createdCart = new ShoppingCartAPI.Models.ShoppingCart
        {
            Id = 1,
            CustomerId = 10,
            Status = 1,
            Created = DateTime.UtcNow
        };
        _mockRepo.Setup(r => r.CreateAsync(10)).ReturnsAsync(createdCart);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(ShoppingCartController.GetById), createdResult.ActionName);
        var returnedCart = Assert.IsType<ShoppingCartAPI.Models.ShoppingCart>(createdResult.Value);
        Assert.Equal(1, returnedCart.Id);
        Assert.Equal(10, returnedCart.CustomerId);
    }

    [Fact]
    public async Task Create_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("CustomerId", "Required");
        var request = new CreateShoppingCartRequest();

        // Act
        var result = await _controller.Create(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    #endregion

    #region AddItem

    [Fact]
    public async Task AddItem_ValidRequest_ReturnsOkWithDetail()
    {
        // Arrange
        var request = new AddItemRequest { ProductId = 5, Quantity = 3 };
        var detail = new ShoppingCartDetail
        {
            Id = 1,
            ShoppingCartId = 1,
            ProductId = 5,
            Quantity = 3,
            Price = 10.00m,
            TotalPrice = 30.00m,
            Status = 1
        };
        _mockRepo.Setup(r => r.AddItemAsync(1, 5, 3)).ReturnsAsync(detail);

        // Act
        var result = await _controller.AddItem(1, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDetail = Assert.IsType<ShoppingCartDetail>(okResult.Value);
        Assert.Equal(5, returnedDetail.ProductId);
        Assert.Equal(3, returnedDetail.Quantity);
        Assert.Equal(30.00m, returnedDetail.TotalPrice);
    }

    [Fact]
    public async Task AddItem_CartOrProductNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new AddItemRequest { ProductId = 5, Quantity = 3 };
        _mockRepo.Setup(r => r.AddItemAsync(999, 5, 3))
                 .ReturnsAsync((ShoppingCartDetail?)null);

        // Act
        var result = await _controller.AddItem(999, request);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddItem_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Quantity", "Required");
        var request = new AddItemRequest();

        // Act
        var result = await _controller.AddItem(1, request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    #endregion

    #region UpdateItem

    [Fact]
    public async Task UpdateItem_ValidRequest_ReturnsOkWithUpdatedDetail()
    {
        // Arrange
        var request = new UpdateItemRequest { Quantity = 5 };
        var detail = new ShoppingCartDetail
        {
            Id = 10,
            ShoppingCartId = 1,
            ProductId = 5,
            Quantity = 5,
            Price = 10.00m,
            TotalPrice = 50.00m,
            Status = 1
        };
        _mockRepo.Setup(r => r.UpdateItemAsync(1, 10, 5)).ReturnsAsync(detail);

        // Act
        var result = await _controller.UpdateItem(1, 10, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDetail = Assert.IsType<ShoppingCartDetail>(okResult.Value);
        Assert.Equal(5, returnedDetail.Quantity);
        Assert.Equal(50.00m, returnedDetail.TotalPrice);
    }

    [Fact]
    public async Task UpdateItem_NotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new UpdateItemRequest { Quantity = 5 };
        _mockRepo.Setup(r => r.UpdateItemAsync(1, 999, 5))
                 .ReturnsAsync((ShoppingCartDetail?)null);

        // Act
        var result = await _controller.UpdateItem(1, 999, request);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateItem_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Quantity", "Required");
        var request = new UpdateItemRequest();

        // Act
        var result = await _controller.UpdateItem(1, 10, request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    #endregion

    #region RemoveItem

    [Fact]
    public async Task RemoveItem_ExistingItem_ReturnsNoContent()
    {
        // Arrange
        _mockRepo.Setup(r => r.RemoveItemAsync(1, 10)).ReturnsAsync(true);

        // Act
        var result = await _controller.RemoveItem(1, 10);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task RemoveItem_NonExistingItem_ReturnsNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.RemoveItemAsync(1, 999)).ReturnsAsync(false);

        // Act
        var result = await _controller.RemoveItem(1, 999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    #endregion
}