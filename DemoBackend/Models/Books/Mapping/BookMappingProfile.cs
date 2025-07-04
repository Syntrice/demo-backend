using DemoBackend.Common.Mapping;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Authors.Responses;
using DemoBackend.Models.Books.Responses;

namespace DemoBackend.Models.Books.Mapping;

public sealed class BookMappingProfile : IMappingProfile
{
    public void Register(Mapper m)
    {
        m.Register<Book, BookResponseModel>(b => new BookResponseModel
        {
            Id = b.Id,
            Title = b.Title
        });

        m.Register<Book, BookDetailsResponseModel>(b => new BookDetailsResponseModel
        {
            Id = b.Id,
            Title = b.Title,
            Authors = m.Map<Author, AuthorResponseModel>(b.Authors)
        });
    }
}