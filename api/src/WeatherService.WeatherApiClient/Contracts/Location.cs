namespace WeatherService.WeatherApiClient.Contracts;

public record Location(
    string Name,
    string Region,
    string Country,
    string LocalTime);