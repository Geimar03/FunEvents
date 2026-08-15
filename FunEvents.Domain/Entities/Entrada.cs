namespace FunEvents.Domain.Entities;

public class Entrada
{
    public Guid Id { get; set; }
    public Guid ReservaId { get; set; }
    public Guid AsientoEventoId { get; set; }
    public string CodigoQr { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public Reserva? Reserva { get; set; }
    public AsientoEvento? AsientoEvento { get; set; }
}
