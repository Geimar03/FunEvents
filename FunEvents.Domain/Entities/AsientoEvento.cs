using FunEvents.Domain.Enums;

namespace FunEvents.Domain.Entities;

public class AsientoEvento
{
    public Guid Id { get; set; }
    public Guid EventoId { get; set; }
    public string CodigoAsiento { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public EstadoAsiento Estado { get; set; }
    public DateTime? ExpiracionReserva { get; set; }
    public Evento? Evento { get; set; }
}
