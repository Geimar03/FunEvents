using FunEvents.Domain.Entities;

namespace FunEvents.Domain.Repositories;

public interface IAsientoEventoRepository
{
    Task<AsientoEvento?> ReservarAsientoAsync(Guid asientoEventoId);
}
