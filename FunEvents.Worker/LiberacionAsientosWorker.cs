using FunEvents.Domain.Entities;
using FunEvents.Domain.Enums;
using FunEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Worker;

public class LiberacionAsientosWorker(ILogger<LiberacionAsientosWorker> _logger, IServiceProvider _proveedorServicios) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Ejecutando Worker de liberación de asientos a las: {Tiempo}", DateTimeOffset.Now);

            try
            {
                using IServiceScope alcance = _proveedorServicios.CreateScope();
                FunEventsDbContext contextoBaseDatos = alcance.ServiceProvider.GetRequiredService<FunEventsDbContext>();

                IQueryable<AsientoEvento> consultaAsientosExpirados = contextoBaseDatos.AsientosEvento
                    .Where(a => a.Estado == EstadoAsiento.Reservado
                             && a.ExpiracionReserva != null
                             && a.ExpiracionReserva < DateTime.UtcNow);

                List<AsientoEvento> asientosExpirados = await consultaAsientosExpirados.ToListAsync(stoppingToken);

                foreach (AsientoEvento asiento in asientosExpirados)
                {
                    asiento.Estado = EstadoAsiento.Disponible;
                    asiento.ExpiracionReserva = null;
                    _logger.LogInformation("Liberado asiento {Codigo} del Evento {Evento}", asiento.CodigoAsiento, asiento.EventoId);
                }

                if (asientosExpirados.Count != 0)
                {
                    await contextoBaseDatos.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Se liberaron {Cantidad} asientos expirados.", asientosExpirados.Count);
                }
            }
            catch (Exception excepcion)
            {
                _logger.LogError(excepcion, "Error durante la liberación de asientos.");
            }

            // Esperar 30 segundos antes del siguiente ciclo
            await Task.Delay(30000, stoppingToken);
        }
    }
}
