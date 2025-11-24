using Microsoft.AspNetCore.Mvc;
using Aprenda.Application.DTOs;
using Aprenda.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aprenda.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TrailController : ControllerBase
    {
        private readonly ITrailService _trailService;

        public TrailController(ITrailService trailService)
        {
            _trailService = trailService;
        }

        // GET api/v1/trail?pageNumber=1&pageSize=5
        [HttpGet]
        public async Task<ActionResult<object>> GetAll(int pageNumber = 1, int pageSize = 5)
        {
            var list = (await _trailService.ListarAsync()).ToList();
            var totalItems = list.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedItems = list
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    t.Id,
                    t.Nome,
                    t.Descricao,
                    Links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(Get), new { id = t.Id, version = "1.0" }) },
                        new { rel = "update", href = Url.Action(nameof(Update), new { id = t.Id, version = "1.0" }) },
                        new { rel = "delete", href = Url.Action(nameof(Delete), new { id = t.Id, version = "1.0" }) }
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

        // GET api/v1/trail/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(Guid id)
        {
            var item = await _trailService.ObterPorIdAsync(id);
            if (item == null) return NotFound();

            var response = new
            {
                item.Id,
                item.Nome,
                item.Descricao,
                Links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(Get), new { id = item.Id, version = "1.0" }) },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id = item.Id, version = "1.0" }) },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id = item.Id, version = "1.0" }) }
                }
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<TrailDto>> Create([FromBody] TrailDto dto)
        {
            var created = await _trailService.CriarAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id, version = "1.0" }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TrailDto dto)
        {
            var ok = await _trailService.AtualizarAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _trailService.DeletarAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
