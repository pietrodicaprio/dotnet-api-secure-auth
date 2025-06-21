using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return Unauthorized(result.Error);

        Response.Cookies.Append("access_token", result.AccessToken, result.AccessCookieOptions);
        Response.Cookies.Append("refresh_token", result.RefreshToken, result.RefreshCookieOptions);

        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var command = new RefreshTokenCommand(Request.Cookies["refresh_token"]);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return Unauthorized(result.Error);

        Response.Cookies.Append("access_token", result.AccessToken, result.AccessCookieOptions);
        return Ok();
    }
}
