namespace ShoppingCartAPI.Contracts
{
    public class ApiResponses
    {
    }

    public record ErrorResponse(string Error, object? Details = null);
    public record SuccessResponse(bool Success = true);


}
