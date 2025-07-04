using DemoBackend.Common.Mapping;
using DemoBackend.Database;
using DemoBackend.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    throw new InvalidOperationException("No connection string configured");
}

builder.Services.AddApplicationDbContext(connectionString);

builder.Services.AddMappersFromAssemblyContaining<Program>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.EnsureDatabaseCreatedAsync();
}

app.UseExceptionHandler(); // Handles unhandled exceptions
app.UseStatusCodePages(); // Handles non-successful status codes
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();