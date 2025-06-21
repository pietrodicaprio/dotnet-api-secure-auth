public static class CookieOptionsFactory
{
    public static CookieOptions CreateAccessTokenOptions() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddMinutes(30)
    };

    public static CookieOptions CreateRefreshTokenOptions(bool rememberMe) => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTimeOffset.UtcNow.AddDays(rememberMe ? 30 : 1)
    };
}
