# FunEvents - Motor de Reserva de Entradas (Prueba Técnica)

Este repositorio contiene la prueba técnica para el diseño e implementación del core transaccional de ventas de FunEvents. El sistema está diseñado para soportar una alta demanda de transacciones concurrentes provenientes de múltiples canales de venta omnicanal.

## 1. Contexto y Visión de Negocio (Arquitectura API-First)

De acuerdo con los requerimientos, el sistema debe ser consumido tanto por el portal web principal de FunEvents como por portales y taquillas de colaboradores externos. 

Para resolver este desafío de integración, se optó por un enfoque **API-First (Headless)**. La aplicación expone un ecosistema de Minimal APIs independientes de la interfaz gráfica. A través de la entidad `CanalVentaId`, el sistema traza exactamente de dónde proviene cada reserva, permitiendo que cualquier socio comercial construya su propia experiencia de usuario (Web, Mobile, POS) mientras consume nuestro motor transaccional centralizado.

## 2. Arquitectura y Tecnologías

El proyecto fue desarrollado utilizando el framework más moderno de Microsoft (**.NET 8** y **C# 12** con tipado estricto) bajo los principios de **Clean Architecture** y **CQRS** (Command Query Responsibility Segregation).

*   **Orquestación:** .NET Aspire (gestión de contenedores, inyección de variables de entorno y telemetría).
*   **Capa de Presentación:** ASP.NET Core Minimal APIs.
*   **Capa de Aplicación:** MediatR para el manejo de comandos y desacoplamiento de casos de uso.
*   **Persistencia:** PostgreSQL gestionado a través de Entity Framework Core.
*   **Procesos en Segundo Plano:** .NET Hosted Services (Background Worker).

## 3. Decisiones Críticas de Diseño

Para garantizar la integridad del sistema frente a escenarios de alta concurrencia (típicos en la venta de boletería), se implementaron las siguientes estrategias de nivel empresarial:

### A. Bloqueo Pesimista y Transacciones Explícitas
En lugar de depender de la concurrencia optimista (que genera cuellos de botella procesando excepciones en el servidor de aplicaciones durante picos de demanda), se delegó la concurrencia al motor ACID de la base de datos.
Se utiliza una transacción explícita (`BeginTransactionAsync`) combinada con un bloqueo de fila a nivel de base de datos (`SELECT ... FOR UPDATE`). Esto encola microscópicamente las peticiones simultáneas, garantizando matemáticamente que un asiento no pueda ser sobrevendido, incluso si dos canales intentan reservarlo en el mismo milisegundo.

### B. Estandarización de Errores (RFC 7807)
Se implementó un `IExceptionHandler` global nativo de .NET 8. La API no arroja excepciones crudas ni Stack Traces; todas las validaciones de negocio y errores de formato son interceptados y devueltos utilizando el estándar internacional **ProblemDetails (RFC 7807)**, facilitando la integración para los desarrolladores Front-end de los colaboradores.

### C. Worker de Liberación Automática
El patrón de reserva temporal asigna el asiento durante 1 minuto (configurable). Un *Background Service* consulta la base de datos de manera asíncrona y devuelve al estado "Disponible" las butacas cuyo proceso de pago no fue completado, asegurando la disponibilidad del inventario en tiempo real.

---

## 4. Guía de Ejecución y Demo de Concurrencia

El proyecto incluye un cliente de consola (`FunEvents.ConsoleClient`) que simula a dos compradores distintos (Portal Web vs. Taquilla Externa) intentando reservar el **mismo asiento exacto** en el **mismo milisegundo**.

### Paso 1: Levantar el Ecosistema
1. Establezca el proyecto **`FunEvents.AppHost`** como proyecto de inicio.
2. Ejecute la solución (F5). Aspire levantará automáticamente el contenedor de PostgreSQL, aplicará las migraciones, sembrará los datos base y desplegará la API junto con el Worker.
3. En el Dashboard de Aspire, localice el endpoint de la API y copie la URL base (Ej. `http://localhost:5288`).

### Paso 2: Simular la Concurrencia
1. Abra una terminal y navegue hasta el directorio del proyecto de consola: `cd FunEvents.ConsoleClient`.
2. Ejecute el simulador pasando la URL de la API como argumento:
   dotnet run http://localhost:<PUERTO_API>
