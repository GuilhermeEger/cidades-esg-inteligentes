using ResiduosApi.Models;

namespace ResiduosApi.Repositories;

public interface ITipoResiduoRepository
{
    Task<IEnumerable<TipoResiduo>> GetAllAsync();
    Task<bool> ExistsAsync(int id);
}
