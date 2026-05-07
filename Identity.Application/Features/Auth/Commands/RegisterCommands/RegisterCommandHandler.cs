using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Features.Auth.Commands.RegisterCommands;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var emailExists = await _userRepository
            .EmailExistsAsync(request.Request.Email);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.Request.FirstName,
            LastName = request.Request.LastName,
            Email = request.Request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Request.Password),
            CreatedAtUtc = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return user.Id;
    }
}