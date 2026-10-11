using Microsoft.EntityFrameworkCore;
using ResiduosApi.Data;
using ResiduosApi.Models;

namespace ResiduosApi.Repositories;

public class TipoResiduoRepository : ITipoResiduoRepository
{
    private readonly AppDbContext _context;
    public TipoResiduoRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<TipoResiduo>> GetAllAsync() =>
        await _context.TiposResiduo.AsNoTracking().OrderBy(t => t.Id).ToListAsync();

    public Task<bool> ExistsAsync(int id) =>
        _context.TiposResiduo.AnyAsync(t => t.Id == id);
}
