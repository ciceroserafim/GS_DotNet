using Microsoft.AspNetCore.Mvc;
using SkillUp.Application.DTOs;
using SkillUp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillUp.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/v1/user?pageNumber=1&pageSize=5
        [HttpGet]
        public async Task<ActionResult<object>> GetAll(int pageNumber = 1, int pageSize = 5)
        {
            var list = (await _userService.ListarAsync()).ToList();
            var totalItems = list.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedItems = list
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.Id,
                    u.Nome,
                    u.Email,
                    Links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(Get), new { id = u.Id, version = "1.0" }) },
                        new { rel = "update", href = Url.Action(nameof(Update), new { id = u.Id, version = "1.0" }) },
                        new { rel = "delete", href = Url.Action(nameof(Delete), new { id = u.Id, version = "1.0" }) }
                    }
                });

            var response = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = pagedItems,
                Links = new List<object>
                {
                    new { rel = "self", href = Url.Action(nameof(GetAll), new { pageNumber, pageSize, version = "1.0" }) },
                    new { rel = "next", href = pageNumber < totalPages ? Url.Action(nameof(GetAll), new { pageNumber = pageNumber + 1, pageSize, version = "1.0" }) : null },
                    new { rel = "prev", href = pageNumber > 1 ? Url.Action(nameof(GetAll), new { pageNumber = pageNumber - 1, pageSize, version = "1.0" }) : null }
                }
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(Guid id)
        {
            var user = await _userService.ObterPorIdAsync(id);
            if (user == null) return NotFound();

            var response = new
            {
                user.Id,
                user.Nome,
                user.Email,
                Links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(Get), new { id = user.Id, version = "1.0" }) },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id = user.Id, version = "1.0" }) },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id = user.Id, version = "1.0" }) }
                }
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDto>> Register([FromBody] UsuarioDto dto)
        {
            var created = await _userService.CriarAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id, version = "1.0" }, created);
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] UsuarioDto dto)
        {
            var list = await _userService.ListarAsync();
            var user = list.FirstOrDefault(u => u.Email == dto.Email && u.Senha == dto.Senha);
            if (user == null) return Unauthorized();

            var response = new
            {
                user.Id,
                user.Nome,
                user.Email,
                Links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(Get), new { id = user.Id, version = "1.0" }) }
                }
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UsuarioDto dto)
        {
            var ok = await _userService.AtualizarAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _userService.DeletarAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
