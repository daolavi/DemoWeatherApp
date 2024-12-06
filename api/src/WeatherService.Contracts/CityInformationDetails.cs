namespace WeatherService.Contracts;

public record CityInformationDetails(
    string City, 
    string Region,
    string Country,
    string LocalTime,
    decimal Temperature,
    string Sunrise,
    string Sunset);