using System.Text.Json.Serialization;

namespace WeatherService.WeatherApiClient.Contracts;

public record CurrentWeatherDetails(Location Location, Weather Current);

public record Weather([property: JsonPropertyName("temp_c")]decimal TempC);