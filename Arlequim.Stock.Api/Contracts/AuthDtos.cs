namespace Arlequim.Stock.Api.Contracts
{
    public record RegisterRequest(string Name, string Email, string Password, string Role); // "Admin" ou "Seller"
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string Token);
}
