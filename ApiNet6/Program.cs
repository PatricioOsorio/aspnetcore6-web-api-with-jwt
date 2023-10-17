using Microsoft.EntityFrameworkCore;
using ApiNet6.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyeccion de dependencias
builder.Services.AddDbContext<DbApiContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
));

// Referencias ciclicas
builder.Services.AddControllers().AddJsonOptions(options =>
  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

// Cors Pt1
var corsRules = "CorsRules";

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: corsRules, builder =>
  {
    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
  });
});

// Configuracion de JWT Pt1
builder.Configuration.AddJsonFile("appsettings.json");
var secretKey = builder.Configuration.GetSection("Settings").GetSection("SecretKey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(config =>
{
  config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
  config.RequireHttpsMetadata = false; // No estamos usando Https
  config.SaveToken = true;
  config.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
    ValidateIssuer = false,
    ValidateAudience = false
  };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}
app.UseSwagger();
app.UseSwaggerUI();

// Cors Pt2
app.UseCors(corsRules);

// Configuracion de JWT Pt2
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
