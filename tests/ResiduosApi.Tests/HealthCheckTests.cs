namespace ResiduosApi.Tests;

/// <summary>Teste do /health — usado pelo Docker e pelos smoke tests do pipeline.</summary>
public class HealthCheckTests : IClassFixture<ResiduosApiFactory>
{
    private readonly ResiduosApiFactory _factory;
    public HealthCheckTests(ResiduosApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Get_Health_RetornaHealthy()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health");
        var body = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode(); // status 200
        Assert.Contains("\"status\":\"Healthy\"", body);
    }
}
