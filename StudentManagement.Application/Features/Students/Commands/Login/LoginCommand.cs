using MediatR;

using StudentManagement.Application.DTO;
namespace StudentManagement.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResultDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}