namespace FunEvents.Domain.Entities;

public class Evento
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaEvento { get; set; }
    public Guid RecintoId { get; set; }
    public Recinto? Recinto { get; set; }
}
