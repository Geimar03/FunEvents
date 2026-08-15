using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Infrastructure.Data;

public class FunEventsDbContext(DbContextOptions<FunEventsDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Recinto> Recintos { get; set; }
    public DbSet<AsientoEvento> AsientosEvento { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Entrada> Entradas { get; set; }
    public DbSet<CanalVenta> CanalesVenta { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recinto>(entidad =>
        {
            entidad.HasKey(e => e.Id);
            entidad.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entidad.Property(e => e.Ubicacion).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<Evento>(entidad =>
        {
            entidad.HasKey(e => e.Id);
            entidad.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entidad.Property(e => e.Descripcion).HasMaxLength(1000);
            entidad.Property(e => e.FechaEvento).IsRequired();

            entidad.HasOne(e => e.Recinto)
                   .WithMany()
                   .HasForeignKey(e => e.RecintoId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AsientoEvento>(entidad =>
        {
            entidad.HasKey(e => e.Id);
            entidad.Property(e => e.CodigoAsiento).IsRequired().HasMaxLength(50);
            entidad.Property(e => e.Precio).HasColumnType("numeric(18,2)");
            entidad.Property(e => e.Estado).IsRequired().HasConversion<string>();

            entidad.HasOne(e => e.Evento)
                   .WithMany()
                   .HasForeignKey(e => e.EventoId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CanalVenta>(entidad =>
        {
            entidad.HasKey(e => e.Id);
            entidad.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entidad.Property(e => e.Tipo).IsRequired().HasConversion<string>();
        });

        modelBuilder.Entity<Usuario>(entidad =>
        {
            entidad.HasKey(e => e.Id);
            entidad.Property(e => e.Correo).IsRequired().HasMaxLength(255);
            entidad.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Reserva>(entidad =>
        {
            entidad.HasKey(e => e.Id);
            entidad.Property(e => e.FechaReserva).IsRequired();
            entidad.Property(e => e.MontoTotal).HasColumnType("numeric(18,2)");
            entidad.Property(e => e.Estado).IsRequired().HasConversion<string>();

            entidad.HasOne(e => e.Usuario)
                   .WithMany()
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            entidad.HasOne(e => e.Canal)
                   .WithMany()
                   .HasForeignKey(e => e.CanalId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Entrada>(entidad =>
        {
            entidad.HasKey(e => e.Id);
            entidad.Property(e => e.CodigoQr).IsRequired().HasMaxLength(500);
            entidad.Property(e => e.FechaEmision).IsRequired();

            entidad.HasOne(e => e.Reserva)
                   .WithMany()
                   .HasForeignKey(e => e.ReservaId)
                   .OnDelete(DeleteBehavior.Restrict);

            entidad.HasOne(e => e.AsientoEvento)
                   .WithMany()
                   .HasForeignKey(e => e.AsientoEventoId)
                   .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
