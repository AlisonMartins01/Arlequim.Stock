using Arlequim.Stock.Api.Contracts;
using Arlequim.Stock.Application.Interfaces;
using Arlequim.Stock.Domain.Entities;
using Arlequim.Stock.Infrastructure.Persistence;
using Arlequim.Stock.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arlequim.Stock.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {

            var token = await authService.RegisterAsync(req.Name, req.Email, req.Password, req.Role);

            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req)
        {
            var response = await authService.LoginAsync(req.Email, req.Password);
            return Ok(new AuthResponse(response));
        }
    }
}
