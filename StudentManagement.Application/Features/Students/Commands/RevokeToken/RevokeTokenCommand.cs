using MediatR;

namespace StudentManagement.Application.Features.Auth.Commands.RevokeToken
{
    public class RevokeTokenCommand : IRequest<Unit>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}