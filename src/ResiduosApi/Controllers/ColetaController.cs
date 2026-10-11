using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResiduosApi.Services;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Controllers;

/// <summary>Registro e consulta de coletas (protegido por JWT).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/coletas")]
[Authorize]
public class ColetaController : ControllerBase
{
    private readonly IColetaService _service;
    public ColetaController(IColetaService service) => _service = service;

    /// <summary>Registra uma coleta de resíduo em um ponto.</summary>
    [HttpPost]
    public async Task<ActionResult<ColetaViewModel>> Post([FromBody] ColetaCreateViewModel vm)
    {
        var criada = await _service.RegistrarAsync(vm);
        return CreatedAtAction(nameof(Get), new { version = "1.0" }, criada);
    }

    /// <summary>Lista paginada com filtros opcionais: ?pontoId=&amp;tipoId=&amp;de=&amp;ate=</summary>
    [HttpGet]
    public async Task<ActionResult<PaginacaoViewModel<ColetaViewModel>>> Get(
        [FromQuery] int? pontoId, [FromQuery] int? tipoId,
        [FromQuery] DateTime? de, [FromQuery] DateTime? ate,
        [FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
    {
        if (pagina < 1) pagina = 1;
        if (tamanho is < 1 or > 100) tamanho = 10;
        return Ok(await _service.ListarAsync(pontoId, tipoId, de, ate, pagina, tamanho));
    }
}
