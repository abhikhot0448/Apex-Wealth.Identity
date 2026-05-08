using FluentAssertions;
using Identity.Application.Features.Auth.Commands.RegisterCommands;
using Identity.Application.Features.Auth.DTOs;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Moq;

namespace Identity.Application.Tests.Features.Auth.Commands;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;

    private readonly Mock<IPasswordHasher> _passwordHasherMock;

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        _passwordHasherMock = new Mock<IPasswordHasher>();

        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new RegisterCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Register_User_Successfully()
    {
        // Arrange
        var request = new RegisterCommand(
            new RegisterRequestDto
            {
                FirstName = "Abhimanyu",
                LastName = "Khot",
                Email = "abhi@test.com",
                Password = "Password123"
            });

        _userRepositoryMock.Setup(x => x.EmailExistsAsync(request.Request.Email)).ReturnsAsync(false);

        _passwordHasherMock
            .Setup(x => x.HashPassword(request.Request.Password))
            .Returns("hashed-password");


        // Act

        var result = await _handler.Handle(
            request,
            CancellationToken.None);


        // Assert
        result.Should().NotBeEmpty();

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<User>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Email_Already_Exists()
    {
        // Arrange
        var request = new RegisterCommand(
            new RegisterRequestDto
            {
                FirstName = "Abhimanyu",
                LastName = "Khot",
                Email = "abhi@test.com",
                Password = "Password123"
            });

        _userRepositoryMock
            .Setup(x => x.EmailExistsAsync(request.Request.Email))
            .ReturnsAsync(true);

        // Act
        Func<Task> action = async () =>
            await _handler.Handle(
                request,
                CancellationToken.None);

        // Assert
        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Email already exists.");
    }
}
