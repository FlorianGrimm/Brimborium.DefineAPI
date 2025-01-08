using Brimborium.DefineAPI;

namespace SampleContracts;

public interface ISampleAPI {
    Task<DARequest<List<WeatherForecast>>> GetWeatherForecast();
}

public partial class DefineResponseListWeatherForecast : DefineResponse<List<WeatherForecast>> { 
}

public partial class ContractDefineAPI: DefineContract<ISampleAPI> { 
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary) {
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

