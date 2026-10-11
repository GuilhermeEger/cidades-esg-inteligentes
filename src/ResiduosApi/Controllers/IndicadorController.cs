using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResiduosApi.Services;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Controllers;

/// <summary>Indicadores agregados de coleta (público) — apoia metas de reciclagem.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/indicadores")]
public class IndicadorController : ControllerBase
{
    private readonly IIndicadorService _service;
    public IndicadorController(IIndicadorService service) => _service = service;

    /// <summary>Total geral coletado (kg) e a quebra por tipo de resíduo.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IndicadorViewModel>> Get()
        => Ok(await _service.ObterAsync());
}
