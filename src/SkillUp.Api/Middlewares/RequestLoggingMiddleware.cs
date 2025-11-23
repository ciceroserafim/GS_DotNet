using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace SkillUp.Api.Middlewares;


public class RequestLoggingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<RequestLoggingMiddleware> _logger;


	public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}


	public async Task Invoke(HttpContext httpContext)
	{
		_logger.LogInformation("Incoming request {method} {path}", httpContext.Request.Method, httpContext.Request.Path);
		await _next(httpContext);
	}
}