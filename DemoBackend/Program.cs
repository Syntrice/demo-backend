using DemoBackend.Common.Mapping;
using DemoBackend.Database;
using DemoBackend.Database.Services;
using DemoBackend.Services;
using DemoBackend.Settings;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Settings

builder.Services
    .AddOptions<DatabaseSettings>()
    .Bind(builder.Configuration.GetSection("DatabaseSettings"))
    .ValidateDataAnnotations();
builder.Services
    .AddOptions<JWTSettings>()
    .Bind(builder.Configuration.GetSection("JWTSettings"))
    .ValidateDataAnnotations();
builder.Services
    .AddOptions<PasswordHashingSettings>()
    .Bind(builder.Configuration.GetSection("PasswordHashingSettings"))
    .ValidateDataAnnotations();

// Utilities

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


// API

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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