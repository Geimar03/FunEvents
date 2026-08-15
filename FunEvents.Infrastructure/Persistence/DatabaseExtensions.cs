using FunEvents.Domain.Entities;
using FunEvents.Domain.Enums;
using FunEvents.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace FunEvents.Infrastructure.Persistence;

public static class DatabaseExtensions
{
    public static async Task InicializarBaseDatosAsync(this IServiceProvider provider)
    {
        using IServiceScope ambito = provider.CreateScope();
        FunEventsDbContext contexto = ambito.ServiceProvider.GetRequiredService<FunEventsDbContext>();
        await contexto.Database.EnsureCreatedAsync();

        if (!contexto.AsientosEvento.Any())
        {
            Usuario usuario = new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Correo = "usuario@funevents.com",
                NombreCompleto = "Usuario de Prueba"
            };

            Usuario usuarioRival = new()
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Correo = "rival@funevents.com",
                NombreCompleto = "Usuario Competidor"
            };

            CanalVenta canal = new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Nombre = "Portal Web Principal",
                Tipo = TipoCanal.PortalWeb,
                Activo = true
            };

            CanalVenta canalTercero = new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Nombre = "Taquilla Externa",
                Tipo = TipoCanal.Taquilla,
                Activo = true
            };

            Recinto recinto = new()
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Nombre = "Estadio Nacional",
                Ubicacion = "Centro de la Ciudad",
                CapacidadMaxima = 50000
            };

            Evento evento = new()
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Nombre = "Gran Concierto de Rock",
                Descripcion = "Concierto internacional",
                FechaEvento = DateTime.UtcNow.AddDays(30),
                RecintoId = recinto.Id
            };

            AsientoEvento asiento1 = new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                EventoId = evento.Id,
                CodigoAsiento = "VIP-01",
                Precio = 150.00m,
                Estado = EstadoAsiento.Disponible
            };

            AsientoEvento asiento2 = new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                EventoId = evento.Id,
                CodigoAsiento = "VIP-02",
                Precio = 150.00m,
                Estado = EstadoAsiento.Disponible
            };

            contexto.Usuarios.Add(usuario);
            contexto.Usuarios.Add(usuarioRival);
            contexto.CanalesVenta.Add(canal);
            contexto.CanalesVenta.Add(canalTercero);
            contexto.Recintos.Add(recinto);
            contexto.Eventos.Add(evento);
            contexto.AsientosEvento.Add(asiento1);
            contexto.AsientosEvento.Add(asiento2);

            await contexto.SaveChangesAsync();
        }
    }
}
