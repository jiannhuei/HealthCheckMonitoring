using Microsoft.Extensions.Configuration;
using Serilog;

namespace HealthCheckMonitoringConsole.Startup
{
    public static class LoggerRegistration
    {
        public static void ConfigureLogger(IConfiguration configuration)
        {
            Log.Logger = new Serilog.LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}