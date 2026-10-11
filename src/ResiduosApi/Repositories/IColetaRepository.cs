using ResiduosApi.Models;

namespace ResiduosApi.Repositories;

public interface IColetaRepository
{
    /// <summary>Lista paginada com filtros opcionais (ponto, tipo, intervalo de datas).</summary>
    Task<(IEnumerable<Coleta> Itens, int Total)> GetFilteredAsync(
        int? pontoId, int? tipoId, DateTime? de, DateTime? ate, int pagina, int tamanho);
    Task AddAsync(Coleta coleta);

    /// <summary>Totais agregados (kg) por tipo de resíduo — base dos indicadores.</summary>
    Task<IEnumerable<(int TipoId, string TipoNome, decimal TotalKg)>> GetTotaisPorTipoAsync();
}
