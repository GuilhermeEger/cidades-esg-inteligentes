namespace ResiduosApi.Security;

/// <summary>
/// Credenciais fixas de acesso à API (seção "Auth"). Simples e suficiente para o
/// escopo da atividade — sem cadastro de usuários (proibido pelo enunciado).
/// </summary>
public class AuthSettings
{
    public string Usuario { get; set; } = "admin";
    public string Senha { get; set; } = "admin123";
}
