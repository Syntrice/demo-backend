using DemoBackend.Models.Authors;

namespace DemoBackend.Models.Books;

public class BookDetailsResponseModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<AuthorResponseModel> Authors { get; set; } = [];
}