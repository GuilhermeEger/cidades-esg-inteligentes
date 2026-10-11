using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResiduosApi.Services;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Controllers;

/// <summary>Tipos de resíduo (catálogo público).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tipos-residuo")]
public class TipoResiduoController : ControllerBase
{
    private readonly ITipoResiduoService _service;
    public TipoResiduoController(ITipoResiduoService service) => _service = service;

    /// <summary>Lista os 6 tipos de resíduo cadastrados.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<TipoResiduoViewModel>>> Get()
        => Ok(await _service.ListarAsync());
}
