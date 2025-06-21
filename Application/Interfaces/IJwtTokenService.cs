public interface IJwtTokenService
{
    string GenerateAccessToken(string uid);
    string GenerateRefreshToken(string uid);
    string? ValidateAccessToken(string token);
}
