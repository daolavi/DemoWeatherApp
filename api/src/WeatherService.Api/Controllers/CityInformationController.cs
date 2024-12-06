using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using WeatherService.Api.Extensions;
using WeatherService.Contracts;
using WeatherService.WeatherApiClient.Client;
using WeatherService.WeatherApiClient.Contracts;

namespace WeatherService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CityInformationController(
    IWeatherApiClient weatherApiClient, 
    IDistributedCache cache,
    ILogger<CityInformationController> logger) : ControllerBase
{
    [ProducesResponseType(typeof(CityInformationDetails), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{city}")]
    public async Task<IActionResult> GetCityInformation(string city, CancellationToken cancellationToken)
    {   
        logger.LogInformation("Getting city information for city: {City}", city);
        
        var currentWeather = await weatherApiClient.GetCurrentWeatherAsync(city, cancellationToken);

        if (currentWeather is null)
        {
            return NotFound(new ProblemDetails { Detail = "Invalid city provided" });
        }
        
        var astro = await cache.GetObjectAsync<Astro>(city, cancellationToken);
        if (astro is null)
        {
            var astronomy = await weatherApiClient.GetAstronomyAsync(city, CancellationToken.None);
            if (astronomy is null)
            {
                return NotFound(new ProblemDetails { Detail = "Invalid city provided" });
            }
            astro = astronomy.Astronomy.Astro;
            await cache.StoreObjectAsync(city, astro, TimeSpan.FromDays(1), cancellationToken);
        }
        
        var result = new CityInformationDetails(
            currentWeather.Location.Name,
            currentWeather.Location.Region,
            currentWeather.Location.Country,
            currentWeather.Location.LocalTime,
            currentWeather.Current.TempC,
            astro.Sunrise,
            astro.Sunset
            );
        return Ok(result);
    }
}
