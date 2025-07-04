using DemoBackend.Models.Books.Responses;

namespace DemoBackend.Models.Authors.Responses;

public class AuthorDetailsResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<BookResponseModel> Books { get; set; } = [];
}