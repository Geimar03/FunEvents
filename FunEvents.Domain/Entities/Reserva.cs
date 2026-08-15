using FunEvents.Domain.Enums;

namespace FunEvents.Domain.Entities;

public class Reserva
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid CanalId { get; set; }
    public DateTime FechaReserva { get; set; }
    public decimal MontoTotal { get; set; }
    public EstadoReserva Estado { get; set; }
    public Usuario? Usuario { get; set; }
    public CanalVenta? Canal { get; set; }
}
