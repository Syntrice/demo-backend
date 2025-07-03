using DemoBackend;
using DemoBackend.Extensions;
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
builder.SetupApplicationDbContext();

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