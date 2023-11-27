using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExceptionHandler.Definitions;
using Serilog;
using Serilog.Events;
using TsylExceptionHandler;
using Microsoft.Extensions.Options;
namespace ExceptionHandler.Extensions;

public static class WebApplicationBuilderExtension
{
    /// <summary>
    /// Configures the IExceptionHandler in .NET 8, using serilog as logger service.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="logSection">The appsettings' section's name where the levels and its paths are defined.</param>
    /// <exception cref="Exception"></exception>
    public static void ConfigureSerilogAndExceptionHandler(this WebApplicationBuilder appBuilder, string logSection)
    {
        appBuilder.Services.AddExceptionHandler<DefaultExceptionHandler>();

        IConfigurationSection valuesSection = appBuilder.Configuration.GetSection(logSection);
        LogConfiguration logConfiguration = [];

        foreach (IConfigurationSection section in valuesSection.GetChildren())
        {
            bool levelParsed = Enum.TryParse(section.GetValue<string>("Level"), out LogEventLevel res);
            bool pathParsed = !string.IsNullOrEmpty(section.GetValue<string>("Path"));

            if (!levelParsed)
                throw new Exception("Please review key 'Level' into appsettings.json. Either no level key is defined or its value is wrong.\n" +
                    "Valid values can be found at: https://github.com/serilog/serilog/wiki/Configuration-Basics");

            if (!pathParsed)
                throw new Exception("The key 'Path' is not defined into appsettings.json, please review it.");

            logConfiguration.Add(new LogLevelPath(res, section.GetValue<string>("Path")));
        }

        ConfigureSerilog(logConfiguration, appBuilder);
    }

    private static void ConfigureSerilog(LogConfiguration logConfiguration, WebApplicationBuilder appBuilder)
    {
        LoggerConfiguration loggerConfiguration = new();

        foreach (var config in logConfiguration)
        {
            switch (config.Key)
            {
                case LogEventLevel.Error:
                    loggerConfiguration.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == config.Key)
                        .WriteTo.File(new JsonErrorFormatter(), config.Value,
                    rollingInterval: RollingInterval.Day));
                    break;
                default:
                    loggerConfiguration.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == config.Key)
                        .WriteTo.File(new JsonDefaultFormatter(), config.Value,
                    rollingInterval: RollingInterval.Day));
                    break;
            }
        }

        appBuilder.Logging.ClearProviders();
        appBuilder.Logging.AddSerilog(loggerConfiguration.CreateLogger());
    }
}


internal class MyLoadconfiguration(IOptions<LogConfiguration> options)
{
    private readonly LogConfiguration _conf = options.Value;

    internal void ConfigureSerilog(WebApplicationBuilder appBuilder)
    {
        LoggerConfiguration loggerConfiguration = new();

        foreach (var config in _conf)
        {
            switch (config.Key)
            {
                case LogEventLevel.Error:
                    loggerConfiguration.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == config.Key)
                        .WriteTo.File(new JsonErrorFormatter(), config.Value,
                    rollingInterval: RollingInterval.Day));
                    break;
                default:
                    loggerConfiguration.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == config.Key)
                        .WriteTo.File(new JsonDefaultFormatter(), config.Value,
                    rollingInterval: RollingInterval.Day));
                    break;
            }
        }

        appBuilder.Logging.ClearProviders();
        appBuilder.Logging.AddSerilog(loggerConfiguration.CreateLogger());
    }
}
