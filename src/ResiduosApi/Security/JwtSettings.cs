namespace ResiduosApi.Security;

/// <summary>Configurações do JWT, carregadas da seção "Jwt" do appsettings.</summary>
public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "ResiduosApi";
    public string Audience { get; set; } = "ResiduosApiClients";
    public int ExpiraMinutos { get; set; } = 60;
}
