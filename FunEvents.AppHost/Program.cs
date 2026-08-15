IDistributedApplicationBuilder constructor = DistributedApplication.CreateBuilder(args);

// Configurar el contenedor de PostgreSQL
IResourceBuilder<PostgresServerResource> postgres = constructor.AddPostgres("postgres")
    .WithPgAdmin();

IResourceBuilder<PostgresDatabaseResource> db = postgres.AddDatabase("funevents");

// Configurar la API y enlazar la base de datos
constructor.AddProject<Projects.FunEvents_Api>("api")
    .WithReference(db)
    .WaitFor(db);

// Configurar el Worker y enlazar la base de datos
constructor.AddProject<Projects.FunEvents_Worker>("worker")
    .WithReference(db)
    .WaitFor(db);

await constructor.Build().RunAsync();
