using System.Text.Json;
using ResiduosApi.Exceptions;

namespace ResiduosApi.Middleware;

/// <summary>
/// Tratamento global de exceções. Converte exceções de domínio em respostas HTTP
/// padronizadas (mesmo contrato JSON da Fase 03: timestamp/status/error/message/path).
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (NotFoundException ex)
        {
            await EscreverErroAsync(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
        }
        catch (BusinessException ex)
        {
            await EscreverErroAsync(context, StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado em {Path}", context.Request.Path);
            await EscreverErroAsync(context, StatusCodes.Status500InternalServerError,
                "Internal Server Error", "Ocorreu um erro inesperado.");
        }
    }

    private static async Task EscreverErroAsync(HttpContext context, int status, string error, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;

        var payload = new
        {
            timestamp = DateTime.UtcNow,
            status,
            error,
            message,
            path = context.Request.Path.Value
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
