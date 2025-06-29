using DemoBackend.Models.Authors;

namespace DemoBackend.Models.Books;

public class BookDetailsResponseModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public IEnumerable<AuthorResponseModel> Authors { get; set; } = [];
}