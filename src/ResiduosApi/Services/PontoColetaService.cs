using AutoMapper;
using ResiduosApi.Exceptions;
using ResiduosApi.Models;
using ResiduosApi.Repositories;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Services;

public interface IPontoColetaService
{
    Task<PaginacaoViewModel<PontoColetaViewModel>> ListarAsync(int pagina, int tamanho);
    Task<PontoColetaViewModel> ObterPorIdAsync(int id);
    Task<PontoColetaViewModel> CriarAsync(PontoColetaCreateViewModel vm);
    Task<PontoColetaViewModel> AtualizarAsync(int id, PontoColetaUpdateViewModel vm);
    Task RemoverAsync(int id);
}

public class PontoColetaService : IPontoColetaService
{
    private readonly IPontoColetaRepository _repo;
    private readonly IMapper _mapper;

    public PontoColetaService(IPontoColetaRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PaginacaoViewModel<PontoColetaViewModel>> ListarAsync(int pagina, int tamanho)
    {
        var (itens, total) = await _repo.GetPagedAsync(pagina, tamanho);
        return new PaginacaoViewModel<PontoColetaViewModel>
        {
            Pagina = pagina,
            Tamanho = tamanho,
            TotalItens = total,
            Itens = _mapper.Map<IEnumerable<PontoColetaViewModel>>(itens)
        };
    }

    public async Task<PontoColetaViewModel> ObterPorIdAsync(int id)
    {
        var ponto = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Ponto de coleta {id} não encontrado.");
        return _mapper.Map<PontoColetaViewModel>(ponto);
    }

    public async Task<PontoColetaViewModel> CriarAsync(PontoColetaCreateViewModel vm)
    {
        var ponto = _mapper.Map<PontoColeta>(vm);
        ponto.Ativo = true;
        ponto.CriadoEm = DateTime.UtcNow;
        await _repo.AddAsync(ponto);
        return _mapper.Map<PontoColetaViewModel>(ponto);
    }

    public async Task<PontoColetaViewModel> AtualizarAsync(int id, PontoColetaUpdateViewModel vm)
    {
        var ponto = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Ponto de coleta {id} não encontrado.");

        // Atualiza os campos preservando Id e CriadoEm originais.
        _mapper.Map(vm, ponto);
        ponto.Id = id;
        await _repo.UpdateAsync(ponto);
        return _mapper.Map<PontoColetaViewModel>(ponto);
    }

    public async Task RemoverAsync(int id)
    {
        var ponto = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Ponto de coleta {id} não encontrado.");
        // Soft-delete: marca como inativo em vez de apagar (preserva histórico de coletas).
        ponto.Ativo = false;
        await _repo.UpdateAsync(ponto);
    }
}
