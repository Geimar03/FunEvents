using FunEvents.Domain.Repositories;
using FunEvents.Infrastructure.Data;
using FunEvents.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunEvents.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuracion)
    {
        string cadenaConexion = configuracion.GetConnectionString("funevents")
            ?? throw new InvalidOperationException("La cadena de conexión 'funevents' no está configurada.");

        services.AddDbContext<FunEventsDbContext>(opciones =>
            opciones.UseNpgsql(cadenaConexion));

        services.AddScoped<IAsientoEventoRepository, AsientoEventoRepository>();

        return services;
    }
}
