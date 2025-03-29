using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace SampleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Tracer _tracer;

    public WeatherController(TracerProvider tracerProvider)
    {
        _tracer = tracerProvider.GetTracer("WeatherAPI");
    }

    [HttpGet("weather")]
    public IActionResult GetWeather()
    {
        using var activity = _tracer.StartActiveSpan("GetWeather");

        var weatherData = Enumerable.Range(1, 5).Select(index => new
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();

        activity.SetAttribute("weather.count", weatherData.Length);
        activity.SetAttribute("weather.sample", weatherData[0].Summary);

        return Ok(weatherData);
    }

    [HttpGet("error")]
    public IActionResult SimulateError()
    {
        using var activity = _tracer.StartActiveSpan("SimulateError");

        try
        {
            throw new Exception("Simulated error for OpenTelemetry");
        }
        catch (Exception ex)
        {
            activity.RecordException(ex);
            return StatusCode(500, new { Message = "An error occurred", Details = ex.Message });
        }
    }
}