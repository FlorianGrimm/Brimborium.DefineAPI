namespace SampleServer;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDefineAPI();
        builder.Services.AddMinimalAPI<SampleAPI>();
        builder.Services.AddSingleton<SampleAPIDefinition>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.MapMinimalAPI();

        app.Run();
    }
}
