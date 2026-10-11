namespace ResiduosApi.Tests;

/// <summary>Teste do ColetaController — endpoint protegido por JWT.</summary>
public class ColetaControllerTests : IClassFixture<ResiduosApiFactory>
{
    private readonly ResiduosApiFactory _factory;
    public ColetaControllerTests(ResiduosApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Get_Coletas_ComToken_RetornaStatus200()
    {
        var client = await AuthHelper.ClienteAutenticadoAsync(_factory);
        var response = await client.GetAsync("/api/v1/coletas");
        response.EnsureSuccessStatusCode(); // status 200
    }
}
