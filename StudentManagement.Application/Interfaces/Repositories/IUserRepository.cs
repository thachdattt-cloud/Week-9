using StudentManagement.Domain.Model;

namespace StudentManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}