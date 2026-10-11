using AutoMapper;
using ResiduosApi.Models;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Mapping;

/// <summary>
/// Centraliza o mapeamento entre entidades de domínio (Model) e ViewModels,
/// mantendo controllers e services livres de código de conversão (Cap 06/07).
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TipoResiduo, TipoResiduoViewModel>();

        CreateMap<PontoColeta, PontoColetaViewModel>();
        CreateMap<PontoColetaCreateViewModel, PontoColeta>();
        CreateMap<PontoColetaUpdateViewModel, PontoColeta>();

        CreateMap<Coleta, ColetaViewModel>()
            .ForMember(d => d.PontoColetaNome, opt => opt.MapFrom(s => s.PontoColeta != null ? s.PontoColeta.Nome : null))
            .ForMember(d => d.TipoResiduoNome, opt => opt.MapFrom(s => s.TipoResiduo != null ? s.TipoResiduo.Nome : null));
        CreateMap<ColetaCreateViewModel, Coleta>();
    }
}
