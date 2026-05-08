namespace Identity.Application.Features.Auth.DTOs;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
}
