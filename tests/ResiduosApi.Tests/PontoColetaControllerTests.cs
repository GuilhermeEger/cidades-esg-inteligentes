using System.Net;

namespace ResiduosApi.Tests;

/// <summary>Teste do PontoColetaController — endpoint protegido por JWT.</summary>
public class PontoColetaControllerTests : IClassFixture<ResiduosApiFactory>
{
    private readonly ResiduosApiFactory _factory;
    public PontoColetaControllerTests(ResiduosApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Get_PontosColeta_ComToken_RetornaStatus200()
    {
        // Arrange — autentica e obtém o Bearer
        var client = await AuthHelper.ClienteAutenticadoAsync(_factory);

        // Act
        var response = await client.GetAsync("/api/v1/pontos-coleta?pagina=1&tamanho=10");

        // Assert
        response.EnsureSuccessStatusCode(); // status 200
    }

    [Fact]
    public async Task Get_PontosColeta_SemToken_RetornaNaoAutorizado()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v1/pontos-coleta");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
