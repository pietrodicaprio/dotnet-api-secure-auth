using ZiggyCreatures.Caching.Fusion;

public class SlidingSessionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IFusionCache _cache;
    private readonly IJwtTokenService _jwtService;

    public SlidingSessionMiddleware(RequestDelegate next, IFusionCache cache, IJwtTokenService jwtService)
    {
        _next = next;
        _cache = cache;
        _jwtService = jwtService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue("access_token", out var token))
        {
            var uid = _jwtService.ValidateAccessToken(token);
            if (!string.IsNullOrEmpty(uid))
            {
                var sessionKey = $"session:{uid}";
                var sessionResult = _cache.TryGet<UserSession>(sessionKey);
                if (sessionResult.HasValue)
                {
                    var session = sessionResult.Value;
                    await _cache.SetAsync(sessionKey, session, TimeSpan.FromMinutes(30));
                    context.Items["UserSession"] = session;
                }
            }
        }

        await _next(context);
    }
}
