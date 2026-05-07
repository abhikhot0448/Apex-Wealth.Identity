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
}
