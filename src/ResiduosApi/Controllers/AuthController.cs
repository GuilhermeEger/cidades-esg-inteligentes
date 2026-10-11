using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResiduosApi.Services;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Controllers;

/// <summary>Autenticação: emite o token JWT usado nos endpoints protegidos.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    public AuthController(ITokenService tokenService) => _tokenService = tokenService;

    /// <summary>Valida credenciais e retorna um token JWT.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<TokenViewModel> Login([FromBody] LoginViewModel login)
    {
        var token = _tokenService.Autenticar(login);
        if (token is null)
            return Unauthorized(new { message = "Usuário ou senha inválidos." });
        return Ok(token);
    }
}
