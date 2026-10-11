using AutoMapper;
using ResiduosApi.Exceptions;
using ResiduosApi.Models;
using ResiduosApi.Repositories;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Services;

public interface IColetaService
{
    Task<ColetaViewModel> RegistrarAsync(ColetaCreateViewModel vm);
    Task<PaginacaoViewModel<ColetaViewModel>> ListarAsync(
        int? pontoId, int? tipoId, DateTime? de, DateTime? ate, int pagina, int tamanho);
}

public class ColetaService : IColetaService
{
    private readonly IColetaRepository _repo;
    private readonly IPontoColetaRepository _pontoRepo;
    private readonly ITipoResiduoRepository _tipoRepo;
    private readonly IMapper _mapper;

    public ColetaService(
        IColetaRepository repo,
        IPontoColetaRepository pontoRepo,
        ITipoResiduoRepository tipoRepo,
        IMapper mapper)
    {
        _repo = repo;
        _pontoRepo = pontoRepo;
        _tipoRepo = tipoRepo;
        _mapper = mapper;
    }

    public async Task<ColetaViewModel> RegistrarAsync(ColetaCreateViewModel vm)
    {
        // Regras de negócio: o ponto precisa existir/estar ativo e o tipo precisa existir.
        if (!await _pontoRepo.ExisteAtivoAsync(vm.PontoColetaId))
            throw new BusinessException($"Ponto de coleta {vm.PontoColetaId} inexistente ou inativo.");
        if (!await _tipoRepo.ExistsAsync(vm.TipoResiduoId))
            throw new BusinessException($"Tipo de resíduo {vm.TipoResiduoId} inexistente.");

        var coleta = _mapper.Map<Coleta>(vm);
        await _repo.AddAsync(coleta);
        return _mapper.Map<ColetaViewModel>(coleta);
    }

    public async Task<PaginacaoViewModel<ColetaViewModel>> ListarAsync(
        int? pontoId, int? tipoId, DateTime? de, DateTime? ate, int pagina, int tamanho)
    {
        var (itens, total) = await _repo.GetFilteredAsync(pontoId, tipoId, de, ate, pagina, tamanho);
        return new PaginacaoViewModel<ColetaViewModel>
        {
            Pagina = pagina,
            Tamanho = tamanho,
            TotalItens = total,
            Itens = _mapper.Map<IEnumerable<ColetaViewModel>>(itens)
        };
    }
}
