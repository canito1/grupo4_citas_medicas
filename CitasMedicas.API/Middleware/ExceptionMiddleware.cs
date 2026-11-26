using CitasMedicas.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace CitasMedicas.API.Middleware;

/// <summary>
/// Middleware centralizado para manejo de excepciones.
/// Captura todas las excepciones no controladas y retorna una respuesta JSON estandarizada.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada: {Mensaje}", ex.Message);
            await ManejarExcepcionAsync(context, ex);
        }
    }

    private static async Task ManejarExcepcionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, mensaje) = ex switch
        {
            NotFoundException => (HttpStatusCode.NotFound, ex.Message),
            ConflictException => (HttpStatusCode.Conflict, ex.Message),
            ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocurrió un error interno en el servidor.")
        };

        context.Response.StatusCode = (int)statusCode;

        var respuesta = new
        {
            status = (int)statusCode,
            error = statusCode.ToString(),
            mensaje
        };

        var opciones = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(respuesta, opciones));
    }
}
