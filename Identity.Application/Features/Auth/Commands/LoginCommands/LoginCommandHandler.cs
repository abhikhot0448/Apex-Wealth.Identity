using Identity.Application.Features.Auth.DTOs;
using Identity.Application.Interfaces;
using Identity.Application.Services;
using MediatR;

namespace Identity.Application.Features.Auth.Commands.LoginCommands;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly JwtTokenService _jwtTokenService;

    public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, JwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Request.Email);

        if (user is null)
        {
            throw new Exception("Invalid email or password.");
        }

        var passwordValid = _passwordHasher.VerifyPassword(request.Request.Password, user.PasswordHash);

        if (!passwordValid)
        {
            throw new Exception("Invalid email or password.");
        }

        var token = _jwtTokenService.GenerateToken(user.Id, user.Email);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Token = token
        };
    }
}
