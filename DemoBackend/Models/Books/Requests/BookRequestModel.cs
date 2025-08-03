namespace DemoBackend.Models.Books.Requests;

public class BookRequestModel
{
    public string Title { get; set; } = string.Empty;
    public IEnumerable<Guid> AuthorIds { get; set; } = [];
}