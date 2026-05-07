using Identity.Application.Features.Auth.DTOs;
using MediatR;

namespace Identity.Application.Features.Auth.Commands.RegisterCommands;

public record RegisterCommand(RegisterRequestDto Request)
 : IRequest<Guid>;
