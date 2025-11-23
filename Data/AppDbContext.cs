using Microsoft.EntityFrameworkCore;
using MentalCheck.API.Models;

namespace MentalCheck.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Checkin> Checkins { get; set; }
    public DbSet<Insight> Insights { get; set; }
    public DbSet<Dica> Dicas { get; set; }
    public DbSet<InsightDica> InsightDicas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("GS_USUARIO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_USUARIO");
            entity.HasIndex(e => e.Email).IsUnique();
            
            entity.HasMany(e => e.Checkins)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(e => e.Insights)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Checkin>(entity =>
        {
            entity.ToTable("GS_CHECKIN");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_CHECKIN");
        });

        modelBuilder.Entity<Insight>(entity =>
        {
            entity.ToTable("GS_INSIGHT");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_INSIGHT");
            
            entity.HasMany(e => e.InsightDicas)
                .WithOne(e => e.Insight)
                .HasForeignKey(e => e.InsightId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Dica>(entity =>
        {
            entity.ToTable("GS_DICA");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_DICA");
            
            entity.HasMany(e => e.InsightDicas)
                .WithOne(e => e.Dica)
                .HasForeignKey(e => e.DicaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InsightDica>(entity =>
        {
            entity.ToTable("GS_INSIGHT_DICA");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID_INSIGHT_DICA");
        });
    }
}
