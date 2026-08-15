namespace FunEvents.Domain.Entities;

public class Recinto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
    public int CapacidadMaxima { get; set; }
}
