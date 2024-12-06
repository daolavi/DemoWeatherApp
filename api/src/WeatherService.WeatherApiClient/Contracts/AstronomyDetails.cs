namespace WeatherService.WeatherApiClient.Contracts;

public record AstronomyDetails(Location Location, Astronomy Astronomy);

public record Astronomy(Astro Astro);

public record Astro(string Sunrise, string Sunset);