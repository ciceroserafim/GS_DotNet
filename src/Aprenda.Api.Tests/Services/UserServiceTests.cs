using FluentAssertions;
using Aprenda.Application.DTOs;
using Aprenda.Application.Implementations;
using Xunit;

namespace Aprenda.Api.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public async Task CriarAsync_DeveCriarUsuarioComIdGerado()
        {
            // Arrange
            var dto = new UsuarioDto
            {
                Nome = "Teste User",
                Email = "teste@example.com",
                Senha = "senha123"
            };

            // Act
            var resultado = await _userService.CriarAsync(dto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Id.Should().NotBeEmpty();
            resultado.Nome.Should().Be(dto.Nome);
            resultado.Email.Should().Be(dto.Email);
        }

        [Fact]
        public async Task ObterPorIdAsync_ComIdExistente_DeveRetornarUsuario()
        {
            // Arrange
            var dto = new UsuarioDto
            {
                Nome = "Teste User",
                Email = "teste@example.com",
                Senha = "senha123"
            };
            var criado = await _userService.CriarAsync(dto);

            // Act
            var resultado = await _userService.ObterPorIdAsync(criado.Id);

            // Assert
            resultado.Should().NotBeNull();
            resultado!.Id.Should().Be(criado.Id);
            resultado.Nome.Should().Be(dto.Nome);
        }

        [Fact]
        public async Task ObterPorIdAsync_ComIdInexistente_DeveRetornarNull()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();

            // Act
            var resultado = await _userService.ObterPorIdAsync(idInexistente);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ListarAsync_DeveRetornarListaDeUsuarios()
        {
            // Arrange
            var dto1 = new UsuarioDto { Nome = "User 1", Email = "user1@test.com", Senha = "senha1" };
            var dto2 = new UsuarioDto { Nome = "User 2", Email = "user2@test.com", Senha = "senha2" };

            await _userService.CriarAsync(dto1);
            await _userService.CriarAsync(dto2);

            // Act
            var resultado = await _userService.ListarAsync();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task AtualizarAsync_ComIdExistente_DeveAtualizarUsuario()
        {
            // Arrange
            var dto = new UsuarioDto
            {
                Nome = "Nome Original",
                Email = "original@test.com",
                Senha = "senha123"
            };
            var criado = await _userService.CriarAsync(dto);

            var dtoAtualizado = new UsuarioDto
            {
                Nome = "Nome Atualizado",
                Email = "atualizado@test.com",
                Senha = "novasenha123"
            };

            // Act
            var resultado = await _userService.AtualizarAsync(criado.Id, dtoAtualizado);

            // Assert
            resultado.Should().BeTrue();
            var usuarioAtualizado = await _userService.ObterPorIdAsync(criado.Id);
            usuarioAtualizado.Should().NotBeNull();
            usuarioAtualizado!.Nome.Should().Be(dtoAtualizado.Nome);
            usuarioAtualizado.Email.Should().Be(dtoAtualizado.Email);
        }

        [Fact]
        public async Task AtualizarAsync_ComIdInexistente_DeveRetornarFalse()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();
            var dto = new UsuarioDto
            {
                Nome = "Teste",
                Email = "teste@test.com",
                Senha = "senha123"
            };

            // Act
            var resultado = await _userService.AtualizarAsync(idInexistente, dto);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public async Task DeletarAsync_ComIdExistente_DeveRemoverUsuario()
        {
            // Arrange
            var dto = new UsuarioDto
            {
                Nome = "User para Deletar",
                Email = "deletar@test.com",
                Senha = "senha123"
            };
            var criado = await _userService.CriarAsync(dto);

            // Act
            var resultado = await _userService.DeletarAsync(criado.Id);

            // Assert
            resultado.Should().BeTrue();
            var usuarioDeletado = await _userService.ObterPorIdAsync(criado.Id);
            usuarioDeletado.Should().BeNull();
        }

        [Fact]
        public async Task DeletarAsync_ComIdInexistente_DeveRetornarFalse()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();

            // Act
            var resultado = await _userService.DeletarAsync(idInexistente);

            // Assert
            resultado.Should().BeFalse();
        }
    }
}

