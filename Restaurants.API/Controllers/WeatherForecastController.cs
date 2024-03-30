using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
    {
        _logger = logger;
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_weatherForecastService.Get());
    }

    [HttpGet("{day:int}")]
    public IActionResult GetOneDayForecast([FromRoute] int day)
    {
        var result = _weatherForecastService.Get(day);
        return Ok(result);

    }

    [HttpPost("generate")]
    public IActionResult Generate([FromQuery] int ResultNumbers,[FromBody] GenerateForecast forecast)
    {
        if (ResultNumbers <= 0 || forecast.MinimumTemprature > forecast.MaximumTemprature)
            return BadRequest();

        return Ok(_weatherForecastService.Generate(ResultNumbers,forecast.MinimumTemprature,forecast.MaximumTemprature));

    }
}
