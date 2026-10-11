namespace ResiduosApi.Tests;

/// <summary>Teste do IndicadorController — endpoint público deve retornar 200.</summary>
public class IndicadorControllerTests : IClassFixture<ResiduosApiFactory>
{
    private readonly ResiduosApiFactory _factory;
    public IndicadorControllerTests(ResiduosApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Get_Indicadores_RetornaStatus200()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v1/indicadores");
        response.EnsureSuccessStatusCode();
    }
}
