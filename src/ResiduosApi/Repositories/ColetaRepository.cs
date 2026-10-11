using Microsoft.EntityFrameworkCore;
using ResiduosApi.Data;
using ResiduosApi.Models;

namespace ResiduosApi.Repositories;

public class ColetaRepository : IColetaRepository
{
    private readonly AppDbContext _context;
    public ColetaRepository(AppDbContext context) => _context = context;

    public async Task<(IEnumerable<Coleta> Itens, int Total)> GetFilteredAsync(
        int? pontoId, int? tipoId, DateTime? de, DateTime? ate, int pagina, int tamanho)
    {
        // Eager loading dos relacionados (Include) + filtros aplicados no banco.
        var query = _context.Coletas
            .AsNoTracking()
            .Include(c => c.PontoColeta)
            .Include(c => c.TipoResiduo)
            .AsQueryable();

        if (pontoId.HasValue) query = query.Where(c => c.PontoColetaId == pontoId.Value);
        if (tipoId.HasValue) query = query.Where(c => c.TipoResiduoId == tipoId.Value);
        if (de.HasValue) query = query.Where(c => c.DataColeta >= de.Value);
        if (ate.HasValue) query = query.Where(c => c.DataColeta <= ate.Value);

        var total = await query.CountAsync();
        var itens = await query
            .OrderByDescending(c => c.DataColeta)
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .ToListAsync();
        return (itens, total);
    }

    public async Task AddAsync(Coleta coleta)
    {
        _context.Coletas.Add(coleta);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<(int TipoId, string TipoNome, decimal TotalKg)>> GetTotaisPorTipoAsync()
    {
        // Agregação executada no banco (GROUP BY) — não traz as linhas para a memória.
        var dados = await _context.Coletas
            .AsNoTracking()
            .GroupBy(c => new { c.TipoResiduoId, c.TipoResiduo!.Nome })
            .Select(g => new { g.Key.TipoResiduoId, g.Key.Nome, Total = g.Sum(x => x.PesoKg) })
            .ToListAsync();

        return dados.Select(d => (d.TipoResiduoId, d.Nome, d.Total));
    }
}
