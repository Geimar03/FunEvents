using FunEvents.Infrastructure.Data;
using FunEvents.Worker;
using Microsoft.EntityFrameworkCore;

HostApplicationBuilder constructor = Host.CreateApplicationBuilder(args);

string cadenaConexion = constructor.Configuration.GetConnectionString("funevents")
    ?? throw new InvalidOperationException("La cadena de conexión 'funevents' no está configurada.");

constructor.Services.AddDbContext<FunEventsDbContext>(opciones =>
    opciones.UseNpgsql(cadenaConexion));

constructor.Services.AddHostedService<LiberacionAsientosWorker>();

IHost anfitrion = constructor.Build();
await anfitrion.RunAsync();
