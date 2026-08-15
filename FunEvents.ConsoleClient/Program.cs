using System.Text;
using System.Text.Json;

namespace FunEvents.ConsoleClient;

internal static class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Iniciando Simulador de Concurrencia de FunEvents...");

        if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
        {
            throw new ArgumentException("Debe proporcionar la URL de la API como primer argumento (ej. http://localhost:5288).");
        }

        string apiUrl = args[0];
        HttpClient clienteHttp = new()
        {
            BaseAddress = new Uri(apiUrl)
        };

        // GUIDs exactos del Seed de datos
        Guid usuarioId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        Guid usuarioRivalId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        Guid asientoId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        Guid canalWebId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        Guid canalTerceroId = Guid.Parse("00000000-0000-0000-0000-000000000002");

        object cuerpoPeticion1 = new
        {
            UsuarioId = usuarioId,
            AsientoId = asientoId,
            CanalVentaId = canalWebId
        };

        object cuerpoPeticion2 = new
        {
            UsuarioId = usuarioRivalId,
            AsientoId = asientoId,
            CanalVentaId = canalTerceroId
        };

        string jsonPayload1 = JsonSerializer.Serialize(cuerpoPeticion1);
        string jsonPayload2 = JsonSerializer.Serialize(cuerpoPeticion2);

        // Crear dos objetos StringContent con canales distintos
        StringContent contenido1 = new(jsonPayload1, Encoding.UTF8, "application/json");
        StringContent contenido2 = new(jsonPayload2, Encoding.UTF8, "application/json");

        Console.WriteLine($"Preparando concurrencia por el Asiento: {asientoId}");
        Console.WriteLine($"Usuario 1: {usuarioId} a través de Canal Web");
        Console.WriteLine($"Usuario 2: {usuarioRivalId} a través de Taquilla Externa");
        Console.WriteLine("Disparando ambas peticiones exactamente al mismo tiempo...");

        try
        {
            // Ejecución paralela sin await inmediato
            Task<HttpResponseMessage> tareaCanal1 = clienteHttp.PostAsync("/api/reservas/reservar", contenido1);
            Task<HttpResponseMessage> tareaCanal2 = clienteHttp.PostAsync("/api/reservas/reservar", contenido2);

            // Esperar ambas peticiones simultáneas
            HttpResponseMessage[] respuestas = await Task.WhenAll(tareaCanal1, tareaCanal2);

            HttpResponseMessage respuesta1 = respuestas[0];
            HttpResponseMessage respuesta2 = respuestas[1];

            string cuerpoRespuesta1 = await respuesta1.Content.ReadAsStringAsync();
            string cuerpoRespuesta2 = await respuesta2.Content.ReadAsStringAsync();

            Console.WriteLine("--- Resultados de la Concurrencia ---");
            Console.WriteLine($"Petición 1 - StatusCode: {(int)respuesta1.StatusCode} {respuesta1.StatusCode}");
            Console.WriteLine($"Petición 1 - Mensaje: {cuerpoRespuesta1}");
            Console.WriteLine();
            Console.WriteLine($"Petición 2 - StatusCode: {(int)respuesta2.StatusCode} {respuesta2.StatusCode}");
            Console.WriteLine($"Petición 2 - Mensaje: {cuerpoRespuesta2}");
        }
        catch (Exception excepcion)
        {
            Console.WriteLine($"Error al conectar con la API: {excepcion.Message}");
        }

        Console.WriteLine("Proceso finalizado. Presione cualquier tecla para salir.");
        Console.ReadLine();
    }
}
