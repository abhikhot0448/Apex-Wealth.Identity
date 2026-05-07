using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IUserRepository
    : IGenericRepository<User>
{
    Task<bool> EmailExistsAsync(string email);

    Task<User?> GetByEmailAsync(string email);
}
