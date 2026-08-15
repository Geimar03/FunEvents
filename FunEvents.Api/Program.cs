using FunEvents.Api.Extensions;
using FunEvents.Api.Handlers;
using FunEvents.Application;
using FunEvents.Infrastructure;
using FunEvents.Infrastructure.Persistence;

WebApplicationBuilder constructor = WebApplication.CreateBuilder(args);

constructor.Services.AddInfrastructure(constructor.Configuration);
constructor.Services.AddApplication();

constructor.Services.AddProblemDetails();
constructor.Services.AddExceptionHandler<ManejadorExcepcionesGlobal>();

constructor.Services.AddEndpointsApiExplorer();
constructor.Services.AddSwaggerGen();

WebApplication app = constructor.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.MapReservaEndpoints();
await app.Services.InicializarBaseDatosAsync();

await app.RunAsync();
