using FunEvents.Domain.Entities;
using FunEvents.Domain.Repositories;
using FunEvents.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Infrastructure.Repositories;

public class AsientoEventoRepository(FunEventsDbContext _dbContext) : IAsientoEventoRepository
{
    public async Task<AsientoEvento?> ReservarAsientoAsync(Guid asientoEventoId)
    {
        IQueryable<AsientoEvento> consulta = _dbContext.AsientosEvento.FromSqlRaw(
            "SELECT * FROM \"AsientosEvento\" WHERE \"Id\" = {0} FOR UPDATE",
            asientoEventoId);

        AsientoEvento? asiento = await consulta.SingleOrDefaultAsync();

        return asiento;
    }
}
