using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTO;
using StudentManagement.Application.Features.Auth.Commands.Login;
using StudentManagement.Application.Features.Auth.Commands.RefreshToken;
using StudentManagement.Application.Features.Auth.Commands.RevokeToken;
using StudentManagement.Shared.Common.ApiResponse;
using System.Security.Claims;

namespace StudentManagement.Application.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResultDto>>> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<LoginResultDto>.Ok(result, "Dang nhap thanh cong"));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<LoginResultDto>>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<LoginResultDto>.Ok(result, "Refresh token thanh cong"));
        }
        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<string>>> Logout([FromBody] RevokeTokenCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<string>.Ok(null!, "Dang xuat thanh cong"));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new { UserId = userId, Username = username, Role = role });
        }
    }
}