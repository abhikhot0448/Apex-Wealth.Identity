using Identity.Application.Features.Auth.DTOs;
using MediatR;

namespace Identity.Application.Features.Auth.Commands.LoginCommands;

public record LoginCommand(LoginRequestDto Request) : IRequest<AuthResponseDto>;
