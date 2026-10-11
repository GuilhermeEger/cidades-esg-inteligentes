using ResiduosApi.Models;

namespace ResiduosApi.Repositories;

public interface IPontoColetaRepository
{
    /// <summary>Lista paginada (apenas ativos), com o total para os metadados.</summary>
    Task<(IEnumerable<PontoColeta> Itens, int Total)> GetPagedAsync(int pagina, int tamanho);
    Task<PontoColeta?> GetByIdAsync(int id);
    Task<bool> ExisteAtivoAsync(int id);
    Task AddAsync(PontoColeta ponto);
    Task UpdateAsync(PontoColeta ponto);
}
