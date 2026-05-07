using Identity.Domain.Common;
using Identity.Domain.Enums;

namespace Identity.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public UserRole Role { get; set; } = UserRole.User;

    public bool IsActive { get; set; } = true;

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTimeUtc { get; set; }
}