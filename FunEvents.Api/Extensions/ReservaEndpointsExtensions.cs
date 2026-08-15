using FunEvents.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunEvents.Api.Extensions;

public static class ReservaEndpointsExtensions
{
    public static void MapReservaEndpoints(this WebApplication app)
    {
        app.MapPost("/api/reservas/reservar", async Task<IResult> ([FromBody] ReservarEntradaCommand comando, IMediator mediador) =>
        {
            Guid? reservaId = await mediador.Send(comando);
            return Results.Ok(new { ReservaId = reservaId, Mensaje = "Reserva creada exitosamente." });
        });
    }
}
