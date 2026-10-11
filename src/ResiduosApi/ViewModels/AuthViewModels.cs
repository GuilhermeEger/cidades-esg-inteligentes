using System.ComponentModel.DataAnnotations;

namespace ResiduosApi.ViewModels;

/// <summary>Credenciais de login para emissão do token JWT.</summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "O usuário é obrigatório.")]
    public string Usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Senha { get; set; } = string.Empty;
}

/// <summary>Token JWT emitido após autenticação bem-sucedida.</summary>
public class TokenViewModel
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiraEm { get; set; }
}
