using DemoBackend.Common.Mapping;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Authors.Responses;
using DemoBackend.Models.Books.Responses;

namespace DemoBackend.Models.Authors.Mapping;

public sealed class AuthorMappingProfile : IMappingProfile
{
    public void Register(Mapper m)
    {
        m.Register<Author, AuthorResponseModel>(a => new AuthorResponseModel
        {
            Id = a.Id,
            Name = a.Name
        });

        m.Register<Author, AuthorDetailsResponseModel>(a => new AuthorDetailsResponseModel
        {
            Id = a.Id,
            Name = a.Name,
            Books = m.Map<Book, BookResponseModel>(a.Books)
        });
    }
}