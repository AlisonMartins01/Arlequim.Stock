using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using Arlequim.Stock.Domain.Exceptions;

namespace Arlequim.Stock.Tests.Auth
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ReturnsOk_WithToken()
        {
            var svc = new Mock<IAuthService>();
            svc.Setup(s => s.LoginAsync("a@b.com", "123", It.IsAny<CancellationToken>()))
               .ReturnsAsync("jwt.token");

            var controller = new AuthController(svc.Object);

            var result = await controller.Login(new LoginRequest { Email = "a@b.com", Password = "123" }, CancellationToken.None);

            var ok = Assert.IsType<OkObjectResult>(result);
            ok.Value.Should().BeEquivalentTo(new { token = "jwt.token" });
        }

        [Fact]
        public async Task Login_InvalidCredentials_IsHandledByMiddleware_As401()
        {
            var svc = new Mock<IAuthService>();
            svc.Setup(s => s.LoginAsync("a@b.com", "wrong", It.IsAny<CancellationToken>()))
               .ThrowsAsync(new InvalidCredentialsException());

            var controller = new AuthController(svc.Object);

            // o controller NÃO trata; a exception sobe pro middleware (no teste unitário, apenas verificamos que ela sobe)
            Func<Task> act = async () =>
                await controller.Login(new LoginRequest { Email = "a@b.com", Password = "wrong" }, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidCredentialsException>();
        }
    }

    // ---- mínimos para compilar o teste ----
    public record LoginRequest { public string Email { get; init; } = ""; public string Password { get; init; } = ""; }
    public interface IAuthService { Task<string> LoginAsync(string email, string password, CancellationToken ct); }
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _svc;
        public AuthController(IAuthService svc) => _svc = svc;

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
        {
            var token = await _svc.LoginAsync(req.Email, req.Password, ct);
            return Ok(new { token });
        }
    }
}
