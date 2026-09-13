using MediatR;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Exceptions;

namespace StudentManagement.Application.Features.Auth.Commands.RevokeToken
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Unit>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RevokeTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

            if (storedToken == null)
            {
                throw new UnauthorizedException("Refresh token khong hop le");
            }

            storedToken.IsRevoked = true;
            await _refreshTokenRepository.SaveChangesAsync();

            return Unit.Value; 
        }
    }
}