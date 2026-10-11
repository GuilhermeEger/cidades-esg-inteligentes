using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResiduosApi.Services;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Controllers;

/// <summary>Pontos de coleta (CRUD protegido por JWT).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pontos-coleta")]
[Authorize]
public class PontoColetaController : ControllerBase
{
    private readonly IPontoColetaService _service;
    public PontoColetaController(IPontoColetaService service) => _service = service;

    /// <summary>Lista paginada de pontos ativos. Ex.: ?pagina=1&amp;tamanho=10</summary>
    [HttpGet]
    public async Task<ActionResult<PaginacaoViewModel<PontoColetaViewModel>>> Get(
        [FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
    {
        if (pagina < 1) pagina = 1;
        if (tamanho is < 1 or > 100) tamanho = 10; // limita o tamanho da página
        return Ok(await _service.ListarAsync(pagina, tamanho));
    }

    /// <summary>Detalhe de um ponto de coleta.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PontoColetaViewModel>> GetById(int id)
        => Ok(await _service.ObterPorIdAsync(id));

    /// <summary>Cria um ponto de coleta.</summary>
    [HttpPost]
    public async Task<ActionResult<PontoColetaViewModel>> Post([FromBody] PontoColetaCreateViewModel vm)
    {
        var criado = await _service.CriarAsync(vm);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id, version = "1.0" }, criado);
    }

    /// <summary>Atualiza um ponto de coleta.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PontoColetaViewModel>> Put(int id, [FromBody] PontoColetaUpdateViewModel vm)
        => Ok(await _service.AtualizarAsync(id, vm));

    /// <summary>Desativa (soft-delete) um ponto de coleta.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }
}
