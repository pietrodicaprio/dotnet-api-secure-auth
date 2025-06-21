using MediatR;
using ZiggyCreatures.Caching.Fusion;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IJwtTokenService _jwtService;
    private readonly IFusionCache _cache;

    public LoginCommandHandler(IJwtTokenService jwtService, IFusionCache cache)
    {
        _jwtService = jwtService;
        _cache = cache;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        string? uid = await ValidateFirebaseTokenAsync(request.FirebaseToken);
        if (uid == null)
            return new LoginResult
            {
                IsSuccess = false,
                Error = "Invalid Firebase token",
                AccessToken = string.Empty,
                RefreshToken = string.Empty,
                AccessCookieOptions = new CookieOptions(),
                RefreshCookieOptions = new CookieOptions()
            };

        var userData = new UserSession { Uid = uid };
        var ttl = TimeSpan.FromMinutes(30);
        _cache.Set($"session:{uid}", userData, ttl);

        var accessToken = _jwtService.GenerateAccessToken(uid);
        var refreshToken = _jwtService.GenerateRefreshToken(uid);

        return new LoginResult
        {
            IsSuccess = true,
            Error = string.Empty,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessCookieOptions = CookieOptionsFactory.CreateAccessTokenOptions(),
            RefreshCookieOptions = CookieOptionsFactory.CreateRefreshTokenOptions(request.RememberMe)
        };
    }

    private Task<string?> ValidateFirebaseTokenAsync(string firebaseToken)
    {
        return Task.FromResult<string?>("uid-example");
    }
}
