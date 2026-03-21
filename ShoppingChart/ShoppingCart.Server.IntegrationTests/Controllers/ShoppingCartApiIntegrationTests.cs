using System.Net;
using System.Net.Http.Json;
using ShoppingCart.Server.IntegrationTests.Fixtures;
using ShoppingCartAPI.Controllers;
using Xunit;

namespace ShoppingCart.Server.IntegrationTests.Controllers;

public class ShoppingCartApiIntegrationTests : IntegrationTestBase
{
    public ShoppingCartApiIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    #region Create

    [Fact]
    public async Task Create_ValidCustomer_ReturnsCreated()
    {
        // Arrange
        var request = new CreateShoppingCartRequest { CustomerId = 1 };

        // Act
        var response = await Client.PostAsJsonAsync("/api/ShoppingCart", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var cart = await response.Content.ReadFromJsonAsync<CartDto>();
        Assert.NotNull(cart);
        Assert.Equal(1, cart.CustomerId);
        Assert.True(cart.Id > 0);
    }

    [Fact]
    public async Task Create_NonExistentCustomer_ReturnsServerError()
    {
        // Arrange — customer 9999 does not exist in seed data
        var request = new CreateShoppingCartRequest { CustomerId = 9999 };

        // Act
        var response = await Client.PostAsJsonAsync("/api/ShoppingCart", request);

        // Assert — repository throws ArgumentException → 500
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    #endregion

    #region GetAll

    [Fact]
    public async Task GetAll_AfterCreating_ReturnsOkWithCarts()
    {
        // Arrange
        await Client.PostAsJsonAsync("/api/ShoppingCart", new CreateShoppingCartRequest { CustomerId = 1 });

        // Act
        var response = await Client.GetAsync("/api/ShoppingCart");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var carts = await response.Content.ReadFromJsonAsync<List<CartDto>>();
        Assert.NotNull(carts);
        Assert.NotEmpty(carts);
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetById_ExistingCart_ReturnsOk()
    {
        // Arrange
        var createResponse = await Client.PostAsJsonAsync("/api/ShoppingCart",
            new CreateShoppingCartRequest { CustomerId = 1 });
        var created = await createResponse.Content.ReadFromJsonAsync<CartDto>();

        // Act
        var response = await Client.GetAsync($"/api/ShoppingCart/{created!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var cart = await response.Content.ReadFromJsonAsync<CartDto>();
        Assert.NotNull(cart);
        Assert.Equal(created.Id, cart.Id);
    }

    [Fact]
    public async Task GetById_NonExistingCart_ReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync("/api/ShoppingCart/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Delete_ExistingCart_ReturnsNoContent()
    {
        // Arrange
        var createResponse = await Client.PostAsJsonAsync("/api/ShoppingCart",
            new CreateShoppingCartRequest { CustomerId = 1 });
        var created = await createResponse.Content.ReadFromJsonAsync<CartDto>();

        // Act
        var response = await Client.DeleteAsync($"/api/ShoppingCart/{created!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NonExistingCart_ReturnsNotFound()
    {
        // Act
        var response = await Client.DeleteAsync("/api/ShoppingCart/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region AddItem

    [Fact]
    public async Task AddItem_ValidRequest_ReturnsOkWithDetail()
    {
        // Arrange
        var createResponse = await Client.PostAsJsonAsync("/api/ShoppingCart",
            new CreateShoppingCartRequest { CustomerId = 1 });
        var cart = await createResponse.Content.ReadFromJsonAsync<CartDto>();

        var addRequest = new AddItemRequest { ProductId = 1, Quantity = 2 };

        // Act
        var response = await Client.PostAsJsonAsync($"/api/ShoppingCart/{cart!.Id}/items", addRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var detail = await response.Content.ReadFromJsonAsync<DetailDto>();
        Assert.NotNull(detail);
        Assert.Equal(1, detail.ProductId);
        Assert.Equal(2, detail.Quantity);
        Assert.Equal(50.00m, detail.TotalPrice); // 25.00 * 2
    }

    [Fact]
    public async Task AddItem_NonExistingCart_ReturnsNotFound()
    {
        // Act
        var response = await Client.PostAsJsonAsync("/api/ShoppingCart/99999/items",
            new AddItemRequest { ProductId = 1, Quantity = 1 });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region UpdateItem

    [Fact]
    public async Task UpdateItem_ValidRequest_ReturnsOkWithUpdatedDetail()
    {
        // Arrange — create cart, add item
        var createResponse = await Client.PostAsJsonAsync("/api/ShoppingCart",
            new CreateShoppingCartRequest { CustomerId = 1 });
        var cart = await createResponse.Content.ReadFromJsonAsync<CartDto>();

        var addResponse = await Client.PostAsJsonAsync($"/api/ShoppingCart/{cart!.Id}/items",
            new AddItemRequest { ProductId = 1, Quantity = 2 });
        var item = await addResponse.Content.ReadFromJsonAsync<DetailDto>();

        // Act — update quantity to 5
        var response = await Client.PutAsJsonAsync(
            $"/api/ShoppingCart/{cart.Id}/items/{item!.Id}",
            new UpdateItemRequest { Quantity = 5 });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await response.Content.ReadFromJsonAsync<DetailDto>();
        Assert.NotNull(updated);
        Assert.Equal(5, updated.Quantity);
        Assert.Equal(125.00m, updated.TotalPrice); // 25.00 * 5
    }

    [Fact]
    public async Task UpdateItem_NonExistingDetail_ReturnsNotFound()
    {
        // Arrange
        var createResponse = await Client.PostAsJsonAsync("/api/ShoppingCart",
            new CreateShoppingCartRequest { CustomerId = 1 });
        var cart = await createResponse.Content.ReadFromJsonAsync<CartDto>();

        // Act
        var response = await Client.PutAsJsonAsync(
            $"/api/ShoppingCart/{cart!.Id}/items/99999",
            new UpdateItemRequest { Quantity = 3 });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region RemoveItem

    [Fact]
    public async Task RemoveItem_ExistingItem_ReturnsNoContent()
    {
        // Arrange
        var createResponse = await Client.PostAsJsonAsync("/api/ShoppingCart",
            new CreateShoppingCartRequest { CustomerId = 1 });
        var cart = await createResponse.Content.ReadFromJsonAsync<CartDto>();

        var addResponse = await Client.PostAsJsonAsync($"/api/ShoppingCart/{cart!.Id}/items",
            new AddItemRequest { ProductId = 1, Quantity = 1 });
        var item = await addResponse.Content.ReadFromJsonAsync<DetailDto>();

        // Act
        var response = await Client.DeleteAsync($"/api/ShoppingCart/{cart.Id}/items/{item!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task RemoveItem_NonExistingItem_ReturnsNotFound()
    {
        // Arrange
        var createResponse = await Client.PostAsJsonAsync("/api/ShoppingCart",
            new CreateShoppingCartRequest { CustomerId = 1 });
        var cart = await createResponse.Content.ReadFromJsonAsync<CartDto>();

        // Act
        var response = await Client.DeleteAsync($"/api/ShoppingCart/{cart!.Id}/items/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Full End-to-End Workflow

    [Fact]
    public async Task FullWorkflow_CreateCart_AddItem_UpdateItem_RemoveItem_DeleteCart()
    {
        // 1. Create a shopping cart
        var createResponse = await Client.PostAsJsonAsync("/api/ShoppingCart",
            new CreateShoppingCartRequest { CustomerId = 1 });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var cart = await createResponse.Content.ReadFromJsonAsync<CartDto>();
        Assert.NotNull(cart);

        // 2. Add an item (quantity = 3, price = 25.00 → total = 75.00)
        var addResponse = await Client.PostAsJsonAsync($"/api/ShoppingCart/{cart.Id}/items",
            new AddItemRequest { ProductId = 1, Quantity = 3 });
        Assert.Equal(HttpStatusCode.OK, addResponse.StatusCode);
        var item = await addResponse.Content.ReadFromJsonAsync<DetailDto>();
        Assert.Equal(75.00m, item!.TotalPrice);

        // 3. Update quantity to 1 → total = 25.00
        var updateResponse = await Client.PutAsJsonAsync(
            $"/api/ShoppingCart/{cart.Id}/items/{item.Id}",
            new UpdateItemRequest { Quantity = 1 });
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        var updated = await updateResponse.Content.ReadFromJsonAsync<DetailDto>();
        Assert.Equal(25.00m, updated!.TotalPrice);

        // 4. Remove the item
        var removeResponse = await Client.DeleteAsync($"/api/ShoppingCart/{cart.Id}/items/{item.Id}");
        Assert.Equal(HttpStatusCode.NoContent, removeResponse.StatusCode);

        // 5. Delete the cart
        var deleteResponse = await Client.DeleteAsync($"/api/ShoppingCart/{cart.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // 6. Verify the cart no longer exists
        var getResponse = await Client.GetAsync($"/api/ShoppingCart/{cart.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    #endregion

    /// <summary>Lightweight DTO for deserializing ShoppingCart JSON responses.</summary>
    private record CartDto(int Id, int CustomerId, int Status);

    /// <summary>Lightweight DTO for deserializing ShoppingCartDetail JSON responses.</summary>
    private record DetailDto(int Id, int ShoppingCartId, int ProductId, int Quantity, decimal Price, decimal TotalPrice);
}