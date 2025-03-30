# OpenTelemetry .NET API with Datadog Agent

This project is a simple example of a .NET API instrumented with OpenTelemetry and exporting traces, logs, and metrics to Datadog Agent.

## Features
- Uses **OpenTelemetry** for distributed tracing, logging, and metrics.
- Sends telemetry data to **Datadog Agent**.
- Includes example endpoints for generating traces, logs, and metrics.

## Prerequisites
- .NET 6 or later
- Docker
- Datadog account (to view telemetry data)

## Setup and Run

### 1. Clone the Repository
```sh
git clone <repo-url>
cd <repo-folder>
```

### 2. Run with Docker Compose
Ensure Docker is running, then execute:
```sh
docker-compose up --build
```
This starts:
- `sampleapi`: The .NET API.
- `datadog-agent`: The Datadog Agent for telemetry collection.

### 3. Verify API is Running
Check the API by visiting:
```
http://localhost:8080/swagger
```

### 4. Check Datadog for Traces, Logs, and Metrics
- Go to [Datadog APM](https://app.datadoghq.com/apm/traces) to see traces.
- View logs in [Datadog Logs](https://app.datadoghq.com/logs).
- Metrics can be found in [Datadog Metrics](https://app.datadoghq.com/metrics).

## API Endpoints

### 1. `GET /api/weather/weather`
Generates weather data and records metrics.

### 2. `GET /api/weather/error`
Simulates an error to test logging and tracing.

### 3. `GET /api/weather/logtest`
Triggers different log levels for testing purposes.

## OpenTelemetry Setup
The API is instrumented with OpenTelemetry using:
- **Tracing**: Captures distributed traces for API requests.
- **Logging**: Sends logs to Datadog.
- **Metrics**: Records request counts and weather-related statistics.

### Configuration in `Program.cs`
```csharp
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("sampleapi"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter());
```

## Environment Variables (Docker)
```yaml
    environment:
      - OTEL_SERVICE_NAME=sampleapi
      - OTEL_EXPORTER_OTLP_ENDPOINT=http://datadog-agent:4317
      - OTEL_EXPORTER_OTLP_PROTOCOL=grpc
      - ASPNETCORE_URLS=http://+:8080
```

## Conclusion
This project demonstrates how to integrate OpenTelemetry with .NET and export telemetry data to Datadog. Use it as a starting point for observability in your applications!

## License
MIT

