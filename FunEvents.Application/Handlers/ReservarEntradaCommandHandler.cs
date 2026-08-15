using FunEvents.Application.Commands;
using FunEvents.Domain.Entities;
using FunEvents.Domain.Enums;
using FunEvents.Domain.Repositories;
using FunEvents.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Application.Handlers;

public class ReservarEntradaCommandHandler(FunEventsDbContext _dbContext, IAsientoEventoRepository _asientoRepository) : IRequestHandler<ReservarEntradaCommand, Guid?>
{
    public async Task<Guid?> Handle(ReservarEntradaCommand request, CancellationToken cancellationToken)
    {
        bool existeUsuario = await _dbContext.Usuarios.AnyAsync(u => u.Id == request.UsuarioId, cancellationToken);
        if (!existeUsuario)
        {
            throw new ArgumentException("El usuario especificado no existe en el sistema.");
        }

        bool existeCanal = await _dbContext.CanalesVenta.AnyAsync(c => c.Id == request.CanalVentaId, cancellationToken);
        if (!existeCanal)
        {
            throw new ArgumentException("El canal de venta especificado no es válido o no existe.");
        }

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaccion = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            AsientoEvento? asientoBloqueado = await _asientoRepository.ReservarAsientoAsync(request.AsientoId) ?? throw new ArgumentException("El asiento especificado no existe.");

            if (asientoBloqueado.Estado != EstadoAsiento.Disponible)
            {
                throw new InvalidOperationException("El asiento ya no se encuentra disponible para reserva.");
            }

            asientoBloqueado.Estado = EstadoAsiento.Reservado;
            asientoBloqueado.ExpiracionReserva = DateTime.UtcNow.AddMinutes(1);

            Reserva nuevaReserva = new()
            {
                Id = Guid.NewGuid(),
                UsuarioId = request.UsuarioId,
                CanalId = request.CanalVentaId,
                FechaReserva = DateTime.UtcNow,
                MontoTotal = asientoBloqueado.Precio,
                Estado = EstadoReserva.Pendiente
            };

            await _dbContext.Reservas.AddAsync(nuevaReserva, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaccion.CommitAsync(cancellationToken);

            return nuevaReserva.Id;
        }
        catch
        {
            await transaccion.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
