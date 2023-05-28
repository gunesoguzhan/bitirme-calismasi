using Meet.Common.RabbitMQ;
using Meet.Common.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Meet.Common.Extensions;

public static class RabbitMQExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();
        ArgumentNullException.ThrowIfNull(configuration);
        var logger = serviceProvider.GetService<ILogger<RabbitMQClient>>();
        ArgumentNullException.ThrowIfNull(logger);
        var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        ArgumentNullException.ThrowIfNull(rabbitMQSettings);
        services.AddSingleton<RabbitMQClient>(provider => new RabbitMQClient(rabbitMQSettings, logger));
        return services;
    }

    public static void UseRabbitMQConsumers(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var serviceProvider = serviceScope.ServiceProvider;
        var rabbitMQ = serviceProvider.GetService<RabbitMQClient>();
        ArgumentNullException.ThrowIfNull(rabbitMQ);
        var consumers = serviceProvider.GetServices<IConsumer>();
        ArgumentNullException.ThrowIfNull(consumers);
        foreach (var consumer in consumers)
            rabbitMQ.Consume(consumer);
    }
}