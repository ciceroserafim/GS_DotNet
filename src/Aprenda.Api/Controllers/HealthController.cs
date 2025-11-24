using Microsoft.AspNetCore.Mvc;

namespace Aprenda.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Verifica o status de saúde da API
        /// </summary>
        /// <returns>Status da aplicação</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok("Healthy");
        }
    }
}

