using DemoBackend.Models.Books.Responses;

namespace DemoBackend.Models.Authors.Responses;

public class AuthorDetailsResponseModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IEnumerable<BookResponseModel> Books { get; set; } = [];
}