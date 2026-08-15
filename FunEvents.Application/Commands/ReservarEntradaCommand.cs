using MediatR;

namespace FunEvents.Application.Commands;

public class ReservarEntradaCommand(Guid usuarioId, Guid asientoId, Guid canalVentaId) : IRequest<Guid?>
{
    public Guid UsuarioId { get; set; } = usuarioId;
    public Guid AsientoId { get; set; } = asientoId;
    public Guid CanalVentaId { get; set; } = canalVentaId;
}
