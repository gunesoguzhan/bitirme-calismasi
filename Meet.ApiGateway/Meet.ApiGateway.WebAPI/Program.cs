using Meet.ApiGateway.WebAPI.Settings;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    var corsSettings = builder.Configuration
        .GetSection(nameof(CorsSettings))
        .Get<CorsSettings>();
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsSettings.AllowedOrigins)
            .WithHeaders(corsSettings.AllowedHeaders)
            .WithMethods(corsSettings.AllowedMethods);
    });
});

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", false, true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseCors();

await app.UseOcelot();

app.Run();
