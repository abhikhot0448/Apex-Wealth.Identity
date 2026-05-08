using Identity.Application.Features.Auth.Commands.LoginCommands;
using Identity.Application.Features.Auth.Commands.RegisterCommands;
using Identity.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var userId = await _mediator.Send(new RegisterCommand(request));

        return Ok(new
        {
            Message = "User registered successfully",
            UserId = userId
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _mediator.Send(new LoginCommand(request));
        return Ok(response);
    }
}
