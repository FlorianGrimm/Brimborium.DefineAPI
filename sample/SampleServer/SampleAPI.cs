using Brimborium.DefineAPI;
using Brimborium.DependencyInjection.Registration;

using SampleContracts;

namespace SampleServer;

public class SampleAPI : ISampleAPI, IMinimalAPI {
{
    public SampleAPI() {

    }
    public void MapEndpoints(IApplicationBuilder app, IEndpointRouteBuilder endpoints) {


        endpoints.MapGet("/weatherforecast", () => {
            var forecast = this.GetWeatherForecast();
            return forecast;
        })
        .WithName("GetWeatherForecast");
    }
       private static string[] summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

    public async Task<DARequest<List<WeatherForecast>>> GetWeatherForecast() {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
        await Task.CompletedTask;
        return DARequest.Create(forecast);
    }
}