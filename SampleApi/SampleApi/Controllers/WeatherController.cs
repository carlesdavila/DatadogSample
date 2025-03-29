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
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(TracerProvider tracerProvider, ILogger<WeatherController> logger)
    {
        _tracer = tracerProvider.GetTracer("WeatherAPI");
        _logger = logger;
    }

    [HttpGet("weather")]
    public IActionResult GetWeather()
    {
        using var activity = _tracer.StartActiveSpan("GetWeather");
        
        // Structured logging with different levels
        _logger.LogInformation("Starting weather data generation");
        _logger.LogDebug("Number of records to generate: {Count}", 5);

        var weatherData = Enumerable.Range(1, 5).Select(index => 
        {
            var temp = Random.Shared.Next(-20, 55);
            var summary = Summaries[Random.Shared.Next(Summaries.Length)];
            
            // Conditional logging
            if (temp > 40)
            {
                _logger.LogWarning("Unusually high temperature detected: {Temperature}C", temp);
            }
            
            return new
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = temp,
                Summary = summary
            };
        }).ToArray();

        // Adding span attributes
        activity.SetAttribute("weather.count", weatherData.Length);
        activity.SetAttribute("weather.sample", weatherData[0].Summary);
        
        _logger.LogInformation("Data generated successfully. First record: {Summary}", weatherData[0].Summary);
        
        return Ok(weatherData);
    }

    [HttpGet("error")]
    public IActionResult SimulateError()
    {
        using var activity = _tracer.StartActiveSpan("SimulateError");
        _logger.LogInformation("Starting error simulation");

        try
        {
            throw new InvalidOperationException("Simulated error for testing");
        }
        catch (Exception ex)
        {
            // Error logging with context
            _logger.LogError(ex, "Simulated error | Path: {Path}", Request.Path);
            activity.RecordException(ex);
            
            return StatusCode(500, new { 
                Message = "Error simulation successful", 
                Details = ex.Message,
                TraceId = activity.Context.TraceId.ToString()
            });
        }
    }

    [HttpGet("logtest")]
    public IActionResult TestLogLevels()
    {
        // Testing all log levels
        _logger.LogTrace("TRACE level message (most detailed)");
        _logger.LogDebug("DEBUG level message");
        _logger.LogInformation("INFORMATION level message");
        _logger.LogWarning("WARNING level message");
        _logger.LogError("ERROR level message");
        _logger.LogCritical("CRITICAL level message");

        return Ok(new { Message = "All log levels tested" });
    }
}