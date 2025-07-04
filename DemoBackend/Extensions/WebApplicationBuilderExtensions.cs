using DemoBackend.Common.Mapping;
using DemoBackend.Database;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Authors.Responses;
using DemoBackend.Models.Books.Responses;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder SetupApplicationDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (connectionString == null)
        {
            throw new InvalidOperationException("No connection string configured");
        }

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            var seeder = new DatabaseSeeder();
            options.UseSeeding((context, _) => { seeder.Seed(context); });
            options.UseAsyncSeeding(async (context, _, cancellationToken) => { await seeder.SeedAsync(context); });
        });
        return builder;
    }

    public static WebApplicationBuilder AddMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMapper>(sp =>
        {
            var mapper = new Mapper();

            mapper.Register<Author, AuthorResponseModel>(author => new AuthorResponseModel()
                { Id = author.Id, Name = author.Name });

            mapper.Register<Author, AuthorDetailsResponseModel>(author => new AuthorDetailsResponseModel()
                { Id = author.Id, Name = author.Name, Books = mapper.Map<Book, BookResponseModel>(author.Books) });

            mapper.Register<Book, BookResponseModel>(book => new BookResponseModel()
                { Id = book.Id, Title = book.Title });

            mapper.Register<Book, BookDetailsResponseModel>(book => new BookDetailsResponseModel()
                { Id = book.Id, Title = book.Title, Authors = mapper.Map<Author, AuthorResponseModel>(book.Authors) });

            return mapper;
        });
        return builder;
    }
}