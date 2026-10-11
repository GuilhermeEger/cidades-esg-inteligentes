using Microsoft.EntityFrameworkCore;
using ResiduosApi.Models;

namespace ResiduosApi.Data;

/// <summary>
/// Contexto EF Core (Code First). Mapeia as entidades para tabelas TB_* e concentra
/// as configurações via Fluent API. As migrações são geradas a partir deste modelo.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PontoColeta> PontosColeta => Set<PontoColeta>();
    public DbSet<TipoResiduo> TiposResiduo => Set<TipoResiduo>();
    public DbSet<Coleta> Coletas => Set<Coleta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ----- TipoResiduo -----
        modelBuilder.Entity<TipoResiduo>(e =>
        {
            e.ToTable("TB_TIPO_RESIDUO");
            e.HasKey(t => t.Id);
            e.Property(t => t.Nome).IsRequired().HasMaxLength(30);
            e.HasIndex(t => t.Nome).IsUnique();
            e.Property(t => t.Cor).HasMaxLength(30);
            e.Property(t => t.InstrucoesDescarte).HasMaxLength(1000);
        });

        // ----- PontoColeta -----
        modelBuilder.Entity<PontoColeta>(e =>
        {
            e.ToTable("TB_PONTO_COLETA");
            e.HasKey(p => p.Id);
            e.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            e.Property(p => p.Endereco).IsRequired().HasMaxLength(200);
            e.Property(p => p.CapacidadeKg).IsRequired().HasColumnType("decimal(10,2)");
            e.Property(p => p.Ativo).IsRequired();
            e.Property(p => p.CriadoEm).IsRequired();
        });

        // ----- Coleta -----
        modelBuilder.Entity<Coleta>(e =>
        {
            e.ToTable("TB_COLETA");
            e.HasKey(c => c.Id);
            e.Property(c => c.PesoKg).IsRequired().HasColumnType("decimal(10,2)");
            e.Property(c => c.DataColeta).IsRequired();
            e.Property(c => c.Responsavel).HasMaxLength(100);

            e.HasOne(c => c.PontoColeta)
                .WithMany(p => p.Coletas)
                .HasForeignKey(c => c.PontoColetaId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(c => c.TipoResiduo)
                .WithMany(t => t.Coletas)
                .HasForeignKey(c => c.TipoResiduoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed dos 6 tipos de resíduo (vai para a migração inicial).
        modelBuilder.Entity<TipoResiduo>().HasData(
            new TipoResiduo { Id = 1, Nome = "PAPEL", Cor = "AZUL", InstrucoesDescarte = "Mantenha seco e limpo; não amasse demais." },
            new TipoResiduo { Id = 2, Nome = "PLASTICO", Cor = "VERMELHO", InstrucoesDescarte = "Lave embalagens antes de descartar." },
            new TipoResiduo { Id = 3, Nome = "VIDRO", Cor = "VERDE", InstrucoesDescarte = "Cuidado com cacos; não descarte espelhos/lâmpadas aqui." },
            new TipoResiduo { Id = 4, Nome = "METAL", Cor = "AMARELO", InstrucoesDescarte = "Latas e tampas limpas." },
            new TipoResiduo { Id = 5, Nome = "ORGANICO", Cor = "MARROM", InstrucoesDescarte = "Restos de alimentos; ideal para compostagem." },
            new TipoResiduo { Id = 6, Nome = "ELETRONICO", Cor = "LARANJA", InstrucoesDescarte = "Leve a pontos de logística reversa específicos." }
        );

        // Dados de exemplo: pontos e coletas para a aplicação já subir com conteúdo
        // (listagens paginadas e indicadores demonstráveis de imediato). Datas fixas
        // porque o seed de migração exige valores determinísticos.
        var criadoEm = new DateTime(2026, 1, 10, 12, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<PontoColeta>().HasData(
            new PontoColeta { Id = 1, Nome = "Ecoponto Centro", Endereco = "Praça da Sé, 1", Latitude = -23.5505, Longitude = -46.6333, CapacidadeKg = 1000m, Ativo = true, CriadoEm = criadoEm },
            new PontoColeta { Id = 2, Nome = "Ecoponto Zona Sul", Endereco = "Av. Ibirapuera, 500", Latitude = -23.5980, Longitude = -46.6560, CapacidadeKg = 750m, Ativo = true, CriadoEm = criadoEm }
        );
        modelBuilder.Entity<Coleta>().HasData(
            new Coleta { Id = 1, PontoColetaId = 1, TipoResiduoId = 1, PesoKg = 120.5m, DataColeta = new DateTime(2026, 2, 1, 9, 0, 0, DateTimeKind.Utc), Responsavel = "Equipe A" },
            new Coleta { Id = 2, PontoColetaId = 1, TipoResiduoId = 2, PesoKg = 80m, DataColeta = new DateTime(2026, 2, 3, 9, 0, 0, DateTimeKind.Utc), Responsavel = "Equipe A" },
            new Coleta { Id = 3, PontoColetaId = 2, TipoResiduoId = 3, PesoKg = 45m, DataColeta = new DateTime(2026, 2, 5, 9, 0, 0, DateTimeKind.Utc), Responsavel = "Equipe B" }
        );
    }
}
