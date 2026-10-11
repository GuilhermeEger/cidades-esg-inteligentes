namespace ResiduosApi.Tests;

/// <summary>Teste do TipoResiduoController — endpoint público deve retornar 200.</summary>
public class TipoResiduoControllerTests : IClassFixture<ResiduosApiFactory>
{
    private readonly ResiduosApiFactory _factory;
    public TipoResiduoControllerTests(ResiduosApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Get_TiposResiduo_RetornaStatus200()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/tipos-residuo");

        // Assert
        response.EnsureSuccessStatusCode(); // status 200
    }
}
