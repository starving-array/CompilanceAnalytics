using System.Threading.Tasks;
using ComplianceAnalytics.Domain.Entities;
using ComplianceAnalytics.Infrastructure.DTO.Auth;
using ComplianceAnalytics.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAnalytics.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterAsync(request.UserName, request.Password, request.Role, request.Region);
            return Ok(new { user.UserName, user.Role, user.Region });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.UserName, request.Password);
            return Ok(new { Token = token });
        }
    }

}
