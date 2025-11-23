using Microsoft.AspNetCore.Mvc;
using SkillUp.Application.DTOs;
using SkillUp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillUp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrailController : ControllerBase
    {
        private readonly ITrailService _trailService;

        public TrailController(ITrailService trailService)
        {
            _trailService = trailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrailDto>>> GetAll()
        {
            var list = await _trailService.ListarAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrailDto>> Get(Guid id)
        {
            var item = await _trailService.ObterPorIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<TrailDto>> Create([FromBody] TrailDto dto)
        {
            var created = await _trailService.CriarAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
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
