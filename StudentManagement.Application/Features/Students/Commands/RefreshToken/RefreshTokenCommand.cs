using MediatR;
using StudentManagement.Application.DTO;

namespace StudentManagement.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<LoginResultDto>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}