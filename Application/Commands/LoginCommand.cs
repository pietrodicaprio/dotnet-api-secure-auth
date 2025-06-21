using MediatR;

public record LoginCommand(string FirebaseToken, bool RememberMe) : IRequest<LoginResult>;

public class LoginResult
{
    public bool IsSuccess { get; set; }
    public string Error { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public CookieOptions AccessCookieOptions { get; set; }
    public CookieOptions RefreshCookieOptions { get; set; }
}
