using DemoBackend.Models.Authors.Responses;

namespace DemoBackend.Models.Books.Responses;

public class BookDetailsResponseModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public IEnumerable<AuthorResponseModel> Authors { get; set; } = [];
}