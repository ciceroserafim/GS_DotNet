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
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET api/v1/courses?pageNumber=1&pageSize=5
        [HttpGet]
        public async Task<ActionResult<object>> GetAll(int pageNumber = 1, int pageSize = 5)
        {
            var list = (await _courseService.ListarAsync()).ToList();
            var totalItems = list.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedItems = list
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new
                {
                    c.Id,
                    c.Nome,
                    c.Descricao,
                    Links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(Get), new { id = c.Id, version = "1.0" }) },
                        new { rel = "update", href = Url.Action(nameof(Update), new { id = c.Id, version = "1.0" }) },
                        new { rel = "delete", href = Url.Action(nameof(Delete), new { id = c.Id, version = "1.0" }) }
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

        // GET api/v1/courses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(Guid id)
        {
            var course = await _courseService.ObterPorIdAsync(id);
            if (course == null) return NotFound();

            var response = new
            {
                course.Id,
                course.Nome,
                course.Descricao,
                Links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(Get), new { id = course.Id, version = "1.0" }) },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id = course.Id, version = "1.0" }) },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id = course.Id, version = "1.0" }) }
                }
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> Create([FromBody] CourseDto dto)
        {
            var created = await _courseService.CriarAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id, version = "1.0" }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseDto dto)
        {
            var ok = await _courseService.AtualizarAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _courseService.DeletarAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
