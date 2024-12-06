using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WeatherService.WeatherApiClient.Contracts;

namespace WeatherService.WeatherApiClient.Client;

public class WeatherApiClient(
    HttpClient httpClient, 
    string apiKey,
    ILogger<WeatherApiClient> logger) : IWeatherApiClient
{
    public async Task<CurrentWeatherDetails?> GetCurrentWeatherAsync(string city, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting current weather for city {City}", city);
        var uri = $"/v1/current.json?key={apiKey}&q={city}";
        var response = await httpClient.GetAsync(uri, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Getting current weather failed for city {City} - statusCode: {StatusCode}", city, response.StatusCode);
            return null;
        }
        var result = await response.Content.ReadFromJsonAsync<CurrentWeatherDetails>(cancellationToken);
        logger.LogInformation("Retrieved current weather for city: {City} - result: {Result}", city, result);
        return result;
    }

    public async Task<AstronomyDetails?> GetAstronomyAsync(string city, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting astronomy for city: {City}", city);
        var uri = $"/v1/astronomy.json?key={apiKey}&q={city}";
        var response = await httpClient.GetAsync(uri, cancellationToken);
        var result = await response.Content.ReadFromJsonAsync<AstronomyDetails>(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Getting astronomy failed for city {City} - statusCode: {StatusCode}", city, response.StatusCode);
            return null;
        }
        logger.LogInformation("Retrieved astronomy for city: {City} - result: {Result}", city, result);
        return result;
    }
}