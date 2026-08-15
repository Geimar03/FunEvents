using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FunEvents.Api.Handlers;

public class ManejadorExcepcionesGlobal(ILogger<ManejadorExcepcionesGlobal> _logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ocurrió un error al procesar la petición: {MensajeError}", exception.Message);

        int codigoEstado;
        string tituloProblema;
        string mensajeAmigable;

        if (exception is ArgumentException || exception is BadHttpRequestException || exception is JsonException || exception is FormatException)
        {
            codigoEstado = StatusCodes.Status400BadRequest;
            tituloProblema = "Error en la petición";
            mensajeAmigable = exception.Message;
        }
        else if (exception is InvalidOperationException)
        {
            codigoEstado = StatusCodes.Status409Conflict;
            tituloProblema = "Conflicto de negocio";
            mensajeAmigable = exception.Message;
        }
        else
        {
            codigoEstado = StatusCodes.Status500InternalServerError;
            tituloProblema = "Error interno del servidor";
            mensajeAmigable = "Ocurrió un error inesperado.";
        }

        ProblemDetails detallesProblema = new()
        {
            Status = codigoEstado,
            Title = tituloProblema,
            Detail = mensajeAmigable,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = codigoEstado;

        await httpContext.Response.WriteAsJsonAsync(detallesProblema, cancellationToken);

        return true;
    }
}
