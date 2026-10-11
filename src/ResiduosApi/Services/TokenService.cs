using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ResiduosApi.Security;
using ResiduosApi.ViewModels;

namespace ResiduosApi.Services;

public interface ITokenService
{
    /// <summary>Valida as credenciais e, se válidas, emite o token JWT.</summary>
    TokenViewModel? Autenticar(LoginViewModel login);
}

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwt;
    private readonly AuthSettings _auth;

    public TokenService(IOptions<JwtSettings> jwt, IOptions<AuthSettings> auth)
    {
        _jwt = jwt.Value;
        _auth = auth.Value;
    }

    public TokenViewModel? Autenticar(LoginViewModel login)
    {
        // Credencial única (sem cadastro de usuários). Comparação direta.
        if (login.Usuario != _auth.Usuario || login.Senha != _auth.Senha)
            return null;

        var expira = DateTime.UtcNow.AddMinutes(_jwt.ExpiraMinutos);
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, login.Usuario),
            new Claim(ClaimTypes.Name, login.Usuario),
            new Claim(ClaimTypes.Role, "admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expira,
            signingCredentials: credenciais);

        return new TokenViewModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiraEm = expira
        };
    }
}
