using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherService.Contracts;
using WeatherService.WeatherApiClient.Contracts;

namespace WeatherService.Api.IntegrationTests.Controllers.CityInformationController;

[TestFixture]
public class GetCityInformationTests : IntegrationTestBase
{
    [Test]
    public async Task Get_WhenCurrentWeatherNotFound_ReturnsNotFound()
    {
        var city = Fixture.Create<string>();
        var astronomy = Fixture.Create<AstronomyDetails>();
        GivenCurrentWeather(null);
        GivenAstronomy(astronomy);
        await WhenCallingEndpoint(city);
        ThenReturnsNotFound();
        await ThenReturnProblemDetails();
    }
    
    [Test]
    public async Task Get_WhenAstronomyNotFound_ReturnsNotFound()
    {
        var city = Fixture.Create<string>();
        var currentWeather = Fixture.Create<CurrentWeatherDetails>();
        GivenCurrentWeather(currentWeather);
        GivenAstronomy(null);
        await WhenCallingEndpoint(city);
        ThenReturnsNotFound();
        await ThenReturnProblemDetails();
    }
    
    [Test]
    public async Task Get_WhenCityValid_ReturnsOk()
    {
        var city = Fixture.Create<string>();
        var currentWeather = Fixture.Create<CurrentWeatherDetails>();
        var astronomy = Fixture.Create<AstronomyDetails>();
        GivenCurrentWeather(currentWeather);
        GivenAstronomy(astronomy);
        await WhenCallingEndpoint(city);
        ThenReturnsOk();
        await ThenReturnCityInformation(currentWeather, astronomy);
    }

    private void GivenCurrentWeather(CurrentWeatherDetails? weatherDetails)
    {
        MockWeatherApiClient
            .Setup(c => c.GetCurrentWeatherAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(weatherDetails);
    }

    private void GivenAstronomy(AstronomyDetails? astronomy)
    {
        MockWeatherApiClient
            .Setup(c => c.GetAstronomyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(astronomy);
    }

    private async Task WhenCallingEndpoint(string city)
    {
        Response = await Sut.GetAsync($"/api/CityInformation/{city}");
    }

    private async Task ThenReturnCityInformation(CurrentWeatherDetails currentWeatherDetails, AstronomyDetails astronomy)
    {
        var expectedResult = new CityInformationDetails(
            currentWeatherDetails.Location.Name,
            currentWeatherDetails.Location.Region,
            currentWeatherDetails.Location.Country,
            currentWeatherDetails.Location.LocalTime,
            currentWeatherDetails.Current.TempC,
            astronomy.Astronomy.Astro.Sunrise,
            astronomy.Astronomy.Astro.Sunset
        );

        var response = await Response.Content.ReadFromJsonAsync<CityInformationDetails>();
        response.Should().BeEquivalentTo(expectedResult);
    }

    private async Task ThenReturnProblemDetails()
    {
        var response = await Response.Content.ReadFromJsonAsync<ProblemDetails>();
        response!.Detail.Should().Be("Invalid city provided");
    }
}