using HealthCheckMonitoringConsole;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

class Program
{
    static void Main(string[] args)
    {

        // Setup Host
        var host = CreateDefaultBuilder().Build();

        // Invoke Worker
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var config = new ConfigurationBuilder()
                .AddJsonFile($"Config/appsettings{(environment == "local" || string.IsNullOrEmpty(environment) ? "" : "." + environment)}.json")
                .Build();
        Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        var workerInstance = provider.GetRequiredService<Worker>();
        workerInstance.DoWork();

        host.Run();
    }

    static IHostBuilder CreateDefaultBuilder()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile($"Config/appsettings{(environment == "local" || string.IsNullOrEmpty(environment) ? "" : "." + environment)}.json");
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<Worker>();
            });
    }
}