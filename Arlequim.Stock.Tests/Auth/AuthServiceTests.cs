using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;

namespace Arlequim.Stock.Tests.Auth
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<ITokenService> _tokenSvc = new();
        private readonly AuthService _sut;

        public AuthServiceTests()
        {
            _sut = new AuthService(_userRepo.Object, _tokenSvc.Object);
        }

        [Fact(DisplayName = "LoginAsync deve lançar InvalidOperationException quando credenciais inválidas (comportamento atual)")]
        public async Task Login_InvalidCredentials_CurrentBehavior_Throws()
        {
            // arrange
            _userRepo
                .Setup(r => r.GetByEmailAsync("a@b.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null!);

            // act
            var act = async () => await _sut.LoginAsync("a@b.com", "wrong", CancellationToken.None);

            // assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Invalid credentials*");
        }

        [Fact(DisplayName = "LoginAsync retorna token quando credenciais válidas")]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            var user = new User { Id = Guid.NewGuid(), Email = "a@b.com", PasswordHash = Hash("123") };

            _userRepo
                .Setup(r => r.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _tokenSvc
                .Setup(t => t.CreateToken(user))
                .Returns("jwt.token.here");

            var token = await _sut.LoginAsync(user.Email, "123", CancellationToken.None);

            token.Should().Be("jwt.token.here");
        }

        // helper fake
        private static string Hash(string s) => $"HASH::{s}";
    }

    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    }
    public interface ITokenService
    {
        string CreateToken(User user);
    }
    public class User { public Guid Id { get; set; } public string Email { get; set; } = ""; public string PasswordHash { get; set; } = ""; }
    public class AuthService
    {
        private readonly IUserRepository _repo; private readonly ITokenService _tk;
        public AuthService(IUserRepository repo, ITokenService tk) { _repo = repo; _tk = tk; }

        public Task<string> LoginAsync(string email, string password, CancellationToken ct)
        {
            return _repo.GetByEmailAsync(email, ct).ContinueWith(t =>
            {
                var u = t.Result;
                if (u is null || u.PasswordHash != $"HASH::{password}")
                    throw new InvalidOperationException("Invalid credentials");

                return _tk.CreateToken(u);
            }, ct);
        }
    }
}
