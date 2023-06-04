using Meet.Common.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilogWithSettings(builder.Configuration);

builder.Services.AddJwtAuthenticationWithSettings().AddCorsWithSettings();

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", false, true)
    // .AddJsonFile("ocelot.json", false, true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// if (app.Environment.IsDevelopment())
app.UseCors();

app.UseAuthentication();

await app.UseOcelot();

app.Run();
