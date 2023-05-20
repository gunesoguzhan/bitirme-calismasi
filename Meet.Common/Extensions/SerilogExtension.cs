using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Meet.Common.Extensions;

public static class SerilogExtension
{
    public static ILoggingBuilder AddSerilogWithSettings(this ILoggingBuilder logging, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration).CreateLogger();
        logging.ClearProviders();
        logging.AddSerilog();
        Serilog.Debugging.SelfLog.Enable(Console.Error);
        return logging;
    }
}