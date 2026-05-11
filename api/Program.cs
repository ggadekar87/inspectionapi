using api.filter;
using api.FluentValidation;
using api.middleware;
using api.model;
using api.Service;
using BankApp.Data.Models;
using BankApp.DTO;
using BankApp.DTO.Validation;
using BankApp.Repos;
using BankApp.Repos.Contrats;
using BankApp.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
// Ensure the Swashbuckle.AspNetCore package is referenced in your project.
// The AddSwaggerGen extension method comes from the Swashbuckle.AspNetCore NuGet package.
// Install-Package Swashbuckle.AspNetCore (or add it via your project file / dotnet add package)
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();//Register HttpClient in DI
builder.Services.AddScoped<CustomAuthenticationFilter>();// Register the custom authentication filter for DI
builder.Services.AddScoped<CustomExceptionFilter>();//  
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBankingService, BankingService>();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddScoped<ITruckAppointmentService, TruckAppointmentService>();
builder.Services.AddScoped<IInspectionService, InspectionService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
#region fluent Validation region
builder.Services.AddFluentValidationAutoValidation();
// Registers all validators in the assembly
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAppointmentRequestValidator>();
builder.Services.AddFluentValidationClientsideAdapters();
#endregion

builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));

#region Session management
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion
builder.Services.AddHealthChecks();

#region Authetication 
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

#endregion
builder.Services.AddDbContext<
    BankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BankDb")));

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB//handle large file uploads and downloads 
});

builder.Services.AddEndpointsApiExplorer();
// Ensure Swashbuckle.AspNetCore is installed; AddSwaggerGen is provided by that package.
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();//Middelware
app.UseAuthentication();//Middelware
app.UseMiddleware<LoggingMiddleware>();//Custome logging middleware
app.UseMiddleware<CustomExceptionMiddleware>();

app.UseCors("AllowAll");  // Apply the policy
app.MapControllers();
app.UseSession();
app.UseHealthChecks("/health");

app.Run();
