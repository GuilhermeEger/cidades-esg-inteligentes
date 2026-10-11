using ResiduosApi.Repositories;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Services;

public interface IIndicadorService
{
    Task<IndicadorViewModel> ObterAsync();
}

public class IndicadorService : IIndicadorService
{
    private readonly IColetaRepository _repo;
    public IndicadorService(IColetaRepository repo) => _repo = repo;

    public async Task<IndicadorViewModel> ObterAsync()
    {
        var totais = await _repo.GetTotaisPorTipoAsync();
        var porTipo = totais
            .Select(t => new TipoTotalViewModel
            {
                TipoResiduoId = t.TipoId,
                TipoResiduoNome = t.TipoNome,
                TotalKg = t.TotalKg
            })
            .OrderByDescending(t => t.TotalKg)
            .ToList();

        return new IndicadorViewModel
        {
            TotalGeralKg = porTipo.Sum(t => t.TotalKg),
            PorTipo = porTipo
        };
    }
}
