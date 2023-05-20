using System.Reflection;
using MassTransit;
using Meet.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meet.Common.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddMassTransitWithSettings(this IServiceCollection services)
    {
        services.AddMassTransit(options =>
        {
            options.AddConsumers(Assembly.GetEntryAssembly());
            options.UsingRabbitMq((context, configurator) =>
            {
                var configuration = context.GetService<IConfiguration>();
                ArgumentNullException.ThrowIfNull(configuration);
                var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                ArgumentNullException.ThrowIfNull(rabbitMQSettings);
                configurator.Host(rabbitMQSettings.Host, h =>
                {
                    h.Username(rabbitMQSettings.Username);
                    h.Password(rabbitMQSettings.Password);
                });
                configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(
                    rabbitMQSettings.ServiceName, false
                ));
            });
        });
        return services;
    }
}