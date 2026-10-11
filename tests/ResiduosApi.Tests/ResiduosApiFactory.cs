using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ResiduosApi.Data;
using ResiduosApi.Models;

namespace ResiduosApi.Tests;

/// <summary>
/// Sobe a API em memória para testes de integração: troca o SQL Server por EF
/// InMemory e popula dados de exemplo, sem depender de banco/Docker externos.
/// </summary>
public class ResiduosApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Ambiente "Testing" desliga a aplicação de migrações no startup (ver Program.cs).
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            // Remove o DbContext configurado com SQL Server.
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.RemoveAll(typeof(AppDbContext));

            // Banco em memória, isolado por nome único.
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("ResiduosTestDb"));

            // Aplica seeds (HasData) e adiciona registros para os testes de listagem.
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();

            if (!db.PontosColeta.Any())
            {
                db.PontosColeta.Add(new PontoColeta
                {
                    Nome = "Ponto Central",
                    Endereco = "Av. Principal, 100",
                    CapacidadeKg = 500m,
                    Ativo = true,
                    CriadoEm = DateTime.UtcNow
                });
                db.SaveChanges();

                var ponto = db.PontosColeta.First();
                db.Coletas.Add(new Coleta
                {
                    PontoColetaId = ponto.Id,
                    TipoResiduoId = 1, // PAPEL (seed)
                    PesoKg = 12.5m,
                    DataColeta = DateTime.UtcNow,
                    Responsavel = "Equipe Teste"
                });
                db.SaveChanges();
            }
        });
    }
}
