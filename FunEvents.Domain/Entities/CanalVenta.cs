using FunEvents.Domain.Enums;

namespace FunEvents.Domain.Entities;

public class CanalVenta
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public TipoCanal Tipo { get; set; }
    public bool Activo { get; set; }
}
