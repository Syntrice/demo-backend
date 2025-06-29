using DemoBackend.Models.Books;

namespace DemoBackend.Models.Authors;

public class AuthorDetailsResponseModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IEnumerable<BookResponseModel> Books { get; set; } = [];
}