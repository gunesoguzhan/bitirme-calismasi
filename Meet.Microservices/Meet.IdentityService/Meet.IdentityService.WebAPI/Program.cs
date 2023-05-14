using Meet.IdentityService.WebAPI.Features.Hashing;
using Meet.IdentityService.WebAPI.Features.Jwt;
using Meet.IdentityService.WebAPI.Common;
using Meet.IdentityService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Meet.IdentityService.WebAPI.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

//Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
Serilog.Debugging.SelfLog.Enable(Console.Error);

// Add services to the container.
builder.Services.AddFluentValidation();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddMassTransit(options =>
{
    options.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMQSettings = builder.Configuration
            .GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        configurator.Host(rabbitMQSettings.Host, h =>
        {
            h.Username(rabbitMQSettings.Username);
            h.Password(rabbitMQSettings.Password);
        });
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(
            "Meet.IdentityService", false
        ));
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
    if (jwtSettings == null) throw new ArgumentNullException();
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidAudience = jwtSettings.Audience,
        ValidIssuer = jwtSettings.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
