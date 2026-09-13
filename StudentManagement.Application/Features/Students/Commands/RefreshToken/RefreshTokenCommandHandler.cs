using MediatR;
using Microsoft.Extensions.Configuration;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Exceptions;
using StudentManagement.Domain.Model;
using RefreshTokenEntity = StudentManagement.Domain.Model.RefreshToken;
namespace StudentManagement.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResultDto>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;

        public RefreshTokenCommandHandler( IRefreshTokenRepository refreshTokenRepository, IJwtTokenGenerator jwtTokenGenerator, IConfiguration configuration)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _configuration = configuration;
        }

        public async Task<LoginResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

            if (storedToken == null)
            {
                throw new UnauthorizedException("Refresh token khong hop le");
            }

            if (storedToken.IsRevoked)
            {
                throw new UnauthorizedException("Refresh token da bi thu hoi");
            }

            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Refresh token da het han");
            }


            storedToken.IsRevoked = true;

            var newAccessToken = _jwtTokenGenerator.GenerateToken(storedToken.User);
            var newRefreshTokenValue = _jwtTokenGenerator.GenerateRefreshToken();

            var refreshTokenExpiryDays = int.Parse(_configuration["JwtSettings:RefreshTokenExpiryDays"]!);

            var newRefreshToken = new RefreshTokenEntity
            {
                Token = newRefreshTokenValue,
                UserID = storedToken.UserID,
                ExpiryDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return new LoginResultDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenValue,
                Username = storedToken.User.Username,
                Role = storedToken.User.Role
            };
        }
    }
}