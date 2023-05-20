using Meet.IdentityService.WebAPI.Features.Hashing;
using Meet.IdentityService.WebAPI.Features.Jwt;
using Meet.IdentityService.WebAPI.Common;
using Meet.IdentityService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Meet.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSerilogWithSettings(builder.Configuration);

// Add services to the container.
builder.Services.AddFluentValidation();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddMassTransitWithSettings();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
