using System.Net;
using System.Net.Http.Json;

namespace ResiduosApi.Tests;

/// <summary>Teste do AuthController — login com credenciais válidas deve retornar 200.</summary>
public class AuthControllerTests : IClassFixture<ResiduosApiFactory>
{
    private readonly ResiduosApiFactory _factory;
    public AuthControllerTests(ResiduosApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Post_Login_ComCredenciaisValidas_RetornaStatus200()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/auth/login",
            new { usuario = "admin", senha = "admin123" });

        // Assert
        response.EnsureSuccessStatusCode(); // status 200
    }

    [Fact]
    public async Task Post_Login_ComCredenciaisInvalidas_RetornaNaoAutorizado()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/auth/login",
            new { usuario = "admin", senha = "errada" });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
