using AutoMapper;
using ResiduosApi.Repositories;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Services;

public interface ITipoResiduoService
{
    Task<IEnumerable<TipoResiduoViewModel>> ListarAsync();
}

public class TipoResiduoService : ITipoResiduoService
{
    private readonly ITipoResiduoRepository _repo;
    private readonly IMapper _mapper;

    public TipoResiduoService(ITipoResiduoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TipoResiduoViewModel>> ListarAsync()
    {
        var tipos = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<TipoResiduoViewModel>>(tipos);
    }
}
