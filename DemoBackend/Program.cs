using System.Text;
using DemoBackend.Common.Mapping;
using DemoBackend.Database;
using DemoBackend.Database.Services;
using DemoBackend.Services;
using DemoBackend.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Utilities

builder.BindSettings();
builder.Services.AddMappersFromAssemblyContaining<Program>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Database

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddHostedService<DatabaseMigrationService>();

// Services

builder.Services.AddScoped<IDatabaseSeedingService, DatabaseSeedingService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IPasswordHashingService, PasswordHashingService>();
builder.Services.AddScoped<IJWTService, JWTService>();

// API

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // We need to add this so that Swagger knows that the API uses JWT tokens, and provides functionality for 
    // sending tokens
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
    };
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            []
        }
    };

    options.AddSecurityRequirement(securityRequirement);
});

// AUTH

var jwtSettingsInstance =
    builder.Configuration.GetSection(JWTSettings.SectionName).Get<JWTSettings>();

if (jwtSettingsInstance == null)
{
    throw new InvalidOperationException(
        "No JWT settings configured. Please provide a JWT configuration section in the appsettings.json file or environment variables.");
}

builder.Services.AddAuthorization(); // used for policy / role / permissions system
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // configure jwt 
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtSettingsInstance.Issuer,
            ValidateIssuer = true,

            ValidAudience = jwtSettingsInstance.Audience,
            ValidateAudience = true,

            ClockSkew = TimeSpan.Zero, // no tolerance for expiration time in the past
            ValidateLifetime = true,

            // Create a signing key using the same secret that is used for token generation
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsInstance.SecretKey)),
            ValidateIssuerSigningKey = true,
        };

        // Implement the abiltiy for the asp.net authentication middleware to get the token from the cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("access-token",
                        out var accessTokenFromCookie))
                {
                    context.Token = accessTokenFromCookie;
                }
                // else, try and get the token from the authorization header

                else if (context.Request.Headers.TryGetValue("Authorization",
                             out var authorizationHeader))
                {
                    var tokenValue = authorizationHeader.FirstOrDefault();
                    if (!string.IsNullOrEmpty(tokenValue) && tokenValue.StartsWith("Bearer"))
                    {
                        context.Token = tokenValue.Substring("Bearer ".Length);
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Needed for integration tests
public partial class Program
{
}