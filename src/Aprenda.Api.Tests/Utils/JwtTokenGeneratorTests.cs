using FluentAssertions;
using Aprenda.Api.Configuration;
using Aprenda.Api.Utils;
using Aprenda.Application.DTOs;
using System;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace Aprenda.Api.Tests.Utils
{
    public class JwtTokenGeneratorTests
    {
        [Fact]
        public void GenerateToken_DeveGerarTokenValido()
        {
            // Arrange
            var user = new UsuarioDto
            {
                Id = Guid.NewGuid(),
                Nome = "Test User",
                Email = "test@test.com"
            };

            var config = new JwtConfig
            {
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                Key = "TestKeyThatIsAtLeast32CharactersLong!",
                ExpiresMinutes = 60
            };

            // Act
            var token = JwtTokenGenerator.GenerateToken(user, config);

            // Assert
            token.Should().NotBeNullOrEmpty();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            jsonToken.Should().NotBeNull();
            jsonToken.Issuer.Should().Be(config.Issuer);
            jsonToken.Audiences.Should().Contain(config.Audience);
        }

        [Fact]
        public void GenerateToken_DeveConterClaimsDoUsuario()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UsuarioDto
            {
                Id = userId,
                Nome = "Test User",
                Email = "test@test.com"
            };

            var config = new JwtConfig
            {
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                Key = "TestKeyThatIsAtLeast32CharactersLong!",
                ExpiresMinutes = 60
            };

            // Act
            var token = JwtTokenGenerator.GenerateToken(user, config);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            jsonToken.Claims.Should().Contain(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier" && c.Value == userId.ToString());
            jsonToken.Claims.Should().Contain(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" && c.Value == user.Email);
        }
    }
}

