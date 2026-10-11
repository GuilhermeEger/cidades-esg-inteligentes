using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ResiduosApi.Tests;

/// <summary>Utilitário: autentica e devolve um HttpClient já com o Bearer configurado.</summary>
public static class AuthHelper
{
    public static async Task<HttpClient> ClienteAutenticadoAsync(ResiduosApiFactory factory)
    {
        var client = factory.CreateClient();
        var resp = await client.PostAsJsonAsync("/api/v1/auth/login",
            new { usuario = "admin", senha = "admin123" });
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();
        var token = JsonDocument.Parse(json).RootElement.GetProperty("token").GetString();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
