using Brimborium.DefineAPI;
using Brimborium.DefineAPI.Server;
using Brimborium.DependencyInjection.Registration;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using SampleContracts;

namespace SampleServer;

public class SampleAPI : ISampleAPI, IMinimalAPI {
    private readonly SampleAPIDefinition _ApiDefinition;

    public SampleAPI(SampleAPIDefinition apiDefinition) {
        this._ApiDefinition = apiDefinition;
    }

    public void MapEndpoints(IApplicationBuilder app, IEndpointRouteBuilder endpoints) {

        endpoints.MapGet("/weatherforecast", async (HttpContext context) => {
            var forecastTask = this.GetWeatherForecast();
            //if (forecast is { }) {
            //    var result= TypedResults.Ok<List<WeatherForecast>>(forecast.Payload);
            //    return result;
            //} else {
            //    return TypedResults.NotFound();
            //}
            //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-9.0
            return _ApiDefinition.WriteGetWeatherForecastAsync(forecastTask, context);
        })
        .WithName("GetWeatherForecast")
        .Produces<List<WeatherForecast>>(200, "application/json")
        ;
    }
    private static string[] summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

    public Task<DARequest<List<WeatherForecast>>> GetWeatherForecast() {
        List<WeatherForecast> forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToList();
        var result = this._ApiDefinition.MetaTypeRepository.CreateDARequestT(forecast);
        return Task.FromResult(result);
    }
}

public class SampleAPIDefinition : ApiDefineContract<ISampleAPI> {
    public SampleAPIDefinition(
        DAMetaTypeRepository metaTypeRepository
        ) {
        this.MetaTypeRepository = metaTypeRepository;
        metaTypeRepository.RegisterResponseHandlerTyped<List<WeatherForecast>>("x", new DAResponseHandlerDelegate<List<WeatherForecast>>("x"));
    }

    public DAMetaTypeRepository MetaTypeRepository { get; }

    // 
    public async Task<Results<Ok<List<WeatherForecast>>, NotFound>> WriteGetWeatherForecastAsync(Task<DARequest<List<WeatherForecast>>> taskValue, HttpContext context) {
        var daRequest = await taskValue;
        //return new ActionResult<List<WeatherForecast>>( daRequest.Payload).Convert();
        return TypedResults.Ok(daRequest.Payload);
    }
}