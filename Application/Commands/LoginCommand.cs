using MediatR;

public record LoginCommand(string FirebaseToken, bool RememberMe) : IRequest<LoginResult>;

public class LoginResult
{
    public bool IsSuccess { get; set; }
    public required string Error { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required CookieOptions AccessCookieOptions { get; set; }
    public required CookieOptions RefreshCookieOptions { get; set; }
}
