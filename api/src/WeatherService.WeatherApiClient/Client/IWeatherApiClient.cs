using WeatherService.WeatherApiClient.Contracts;

namespace WeatherService.WeatherApiClient.Client;

public interface IWeatherApiClient
{
    Task<CurrentWeatherDetails?> GetCurrentWeatherAsync(string city, CancellationToken cancellationToken = default);
    Task<AstronomyDetails?> GetAstronomyAsync(string city, CancellationToken cancellationToken = default);
}