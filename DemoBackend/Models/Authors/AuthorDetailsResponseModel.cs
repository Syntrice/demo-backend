using DemoBackend.Models.Books;

namespace DemoBackend.Models.Authors;

public class AuthorDetailsResponseModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<BookResponseModel> Books { get; set; } = [];
}