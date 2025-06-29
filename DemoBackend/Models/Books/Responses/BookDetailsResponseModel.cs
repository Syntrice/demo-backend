using DemoBackend.Models.Authors;
using DemoBackend.Models.Authors.Responses;

namespace DemoBackend.Models.Books.Responses;

public class BookDetailsResponseModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public IEnumerable<AuthorResponseModel> Authors { get; set; } = [];
}