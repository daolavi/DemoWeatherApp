using System.Net;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeatherService.WeatherApiClient.Client;

namespace WeatherService.Api.IntegrationTests.Controllers;

public class IntegrationTestBase
{
    private WeatherServiceApiApplicationFactory<Program> _factory = null!;
    protected HttpClient Sut = null!;
    protected HttpResponseMessage Response = null!;
    protected readonly Fixture Fixture = new();
    protected readonly Mock<IWeatherApiClient> MockWeatherApiClient = new();

    [SetUp]
    protected void Setup()
    {
        _factory = new WeatherServiceApiApplicationFactory<Program>();
        Sut = _factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IWeatherApiClient>(sp => MockWeatherApiClient.Object);
                });
            })
            .CreateClient();
    }
    
    [TearDown]
    protected async Task TearDown()
    {
        Sut.Dispose();
        await _factory.DisposeAsync();
    }

    protected void ThenReturnsOk()
    {
        Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    protected void ThenReturnsNotFound()
    {
        Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}