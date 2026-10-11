using Microsoft.EntityFrameworkCore;
using ResiduosApi.Data;
using ResiduosApi.Models;

namespace ResiduosApi.Repositories;

public class PontoColetaRepository : IPontoColetaRepository
{
    private readonly AppDbContext _context;
    public PontoColetaRepository(AppDbContext context) => _context = context;

    public async Task<(IEnumerable<PontoColeta> Itens, int Total)> GetPagedAsync(int pagina, int tamanho)
    {
        // Otimização de consulta: filtra ativos, ordena e pagina no banco (Skip/Take),
        // sem rastreamento de mudanças (AsNoTracking) por ser leitura.
        var query = _context.PontosColeta.AsNoTracking().Where(p => p.Ativo);
        var total = await query.CountAsync();
        var itens = await query
            .OrderBy(p => p.Nome)
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .ToListAsync();
        return (itens, total);
    }

    public Task<PontoColeta?> GetByIdAsync(int id) =>
        _context.PontosColeta.FirstOrDefaultAsync(p => p.Id == id && p.Ativo);

    public Task<bool> ExisteAtivoAsync(int id) =>
        _context.PontosColeta.AnyAsync(p => p.Id == id && p.Ativo);

    public async Task AddAsync(PontoColeta ponto)
    {
        _context.PontosColeta.Add(ponto);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PontoColeta ponto)
    {
        _context.PontosColeta.Update(ponto);
        await _context.SaveChangesAsync();
    }
}
