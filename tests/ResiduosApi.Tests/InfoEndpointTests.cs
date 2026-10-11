namespace ResiduosApi.Tests;

/// <summary>Teste do GET / — identifica ambiente e versão publicada.</summary>
public class InfoEndpointTests : IClassFixture<ResiduosApiFactory>
{
    private readonly ResiduosApiFactory _factory;
    public InfoEndpointTests(ResiduosApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Get_Raiz_RetornaAmbienteEVersao()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");
        var body = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode(); // status 200
        Assert.Contains("\"ambiente\":\"Testing\"", body);
        Assert.Contains("\"versao\"", body);
    }
}
