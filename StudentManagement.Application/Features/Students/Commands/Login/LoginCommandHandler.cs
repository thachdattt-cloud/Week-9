using MediatR;
using Microsoft.Extensions.Configuration;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Exceptions;
using StudentManagement.Domain.Model;
using RefreshTokenEntity = StudentManagement.Domain.Model.RefreshToken;
namespace StudentManagement.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
        }

        public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                throw new UnauthorizedException("Sai username hoac password");
            }

            bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new UnauthorizedException("Sai username hoac password");
            }

            var accessToken = _jwtTokenGenerator.GenerateToken(user);
            var refreshTokenValue = _jwtTokenGenerator.GenerateRefreshToken();

            var refreshTokenExpiryDays = int.Parse(_configuration["JwtSettings:RefreshTokenExpiryDays"]!);

            var refreshToken = new RefreshTokenEntity
            {
                Token = refreshTokenValue,
                UserID = user.UserID,
                ExpiryDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return new LoginResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}