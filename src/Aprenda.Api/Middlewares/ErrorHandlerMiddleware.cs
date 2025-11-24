using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;


namespace Aprenda.Api.Middlewares;


public class ErrorHandlerMiddleware
{
	private readonly RequestDelegate _next;


	public ErrorHandlerMiddleware(RequestDelegate next) => _next = next;


	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


			var response = new { message = "An unexpected error occurred.", detail = ex.Message };
			var json = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(json);
		}
	}
}