using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using Aprenda.Api.Configuration;
using Aprenda.Api.Controllers;
using Aprenda.Application.DTOs;
using Aprenda.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Aprenda.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;
        private readonly JwtConfig _jwtConfig;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _jwtConfig = new JwtConfig
            {
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                Key = "TestKeyThatIsAtLeast32CharactersLong!",
                ExpiresMinutes = 60
            };

            var jwtConfigOptions = Options.Create(_jwtConfig);
            _controller = new UserController(_mockUserService.Object, jwtConfigOptions);

            // Configurar contexto mínimo para UrlHelper
            var httpContext = new DefaultHttpContext();
            var routeData = new RouteData();
            var actionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor();
            var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);
            _controller.ControllerContext = new ControllerContext(actionContext);

            // Mock do UrlHelper
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/v1/user/test");
            _controller.Url = urlHelper.Object;
        }

        [Fact]
        public async Task GetAll_DeveRetornarOkComListaPaginada()
        {
            // Arrange
            var usuarios = new List<UsuarioDto>
            {
                new UsuarioDto { Id = Guid.NewGuid(), Nome = "User 1", Email = "user1@test.com" },
                new UsuarioDto { Id = Guid.NewGuid(), Nome = "User 2", Email = "user2@test.com" },
                new UsuarioDto { Id = Guid.NewGuid(), Nome = "User 3", Email = "user3@test.com" }
            };

            _mockUserService.Setup(s => s.ListarAsync())
                .ReturnsAsync(usuarios);

            // Act
            var resultado = await _controller.GetAll(1, 2);

            // Assert
            resultado.Result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)resultado.Result!;
            okResult.Value.Should().NotBeNull();

            // Verificar que a resposta contém as propriedades esperadas usando reflexão
            var responseType = okResult.Value!.GetType();
            responseType.GetProperty("Items").Should().NotBeNull();
            responseType.GetProperty("PageNumber").Should().NotBeNull();
            responseType.GetProperty("TotalItems").Should().NotBeNull();
        }

        [Fact]
        public async Task Get_ComIdExistente_DeveRetornarOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var usuario = new UsuarioDto
            {
                Id = id,
                Nome = "Test User",
                Email = "test@test.com"
            };

            _mockUserService.Setup(s => s.ObterPorIdAsync(id))
                .ReturnsAsync(usuario);

            // Act
            var resultado = await _controller.Get(id);

            // Assert
            resultado.Result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)resultado.Result!;
            okResult.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_ComIdInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockUserService.Setup(s => s.ObterPorIdAsync(id))
                .ReturnsAsync((UsuarioDto?)null);

            // Act
            var resultado = await _controller.Get(id);

            // Assert
            resultado.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Register_DeveRetornarCreated()
        {
            // Arrange
            var dto = new UsuarioDto
            {
                Nome = "New User",
                Email = "new@test.com",
                Senha = "senha123"
            };

            var usuarioCriado = new UsuarioDto
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha
            };

            _mockUserService.Setup(s => s.CriarAsync(It.IsAny<UsuarioDto>()))
                .ReturnsAsync(usuarioCriado);

            // Act
            var resultado = await _controller.Register(dto);

            // Assert
            resultado.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = (CreatedAtActionResult)resultado.Result!;
            createdResult.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Update_ComIdExistente_DeveRetornarNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new UsuarioDto
            {
                Nome = "Updated Name",
                Email = "updated@test.com",
                Senha = "newsenha"
            };

            _mockUserService.Setup(s => s.AtualizarAsync(id, dto))
                .ReturnsAsync(true);

            // Act
            var resultado = await _controller.Update(id, dto);

            // Assert
            resultado.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Update_ComIdInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new UsuarioDto { Nome = "Test", Email = "test@test.com", Senha = "senha" };

            _mockUserService.Setup(s => s.AtualizarAsync(id, dto))
                .ReturnsAsync(false);

            // Act
            var resultado = await _controller.Update(id, dto);

            // Assert
            resultado.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_ComIdExistente_DeveRetornarNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockUserService.Setup(s => s.DeletarAsync(id))
                .ReturnsAsync(true);

            // Act
            var resultado = await _controller.Delete(id);

            // Assert
            resultado.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ComIdInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockUserService.Setup(s => s.DeletarAsync(id))
                .ReturnsAsync(false);

            // Act
            var resultado = await _controller.Delete(id);

            // Assert
            resultado.Should().BeOfType<NotFoundResult>();
        }
    }
}

