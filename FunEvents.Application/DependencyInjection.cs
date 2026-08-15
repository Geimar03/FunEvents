using FunEvents.Application.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace FunEvents.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuracion =>
            configuracion.RegisterServicesFromAssembly(typeof(ReservarEntradaCommand).Assembly));

        return services;
    }
}
