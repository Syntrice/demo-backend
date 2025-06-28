namespace DemoBackend.Database.Entities;

public class Book : IEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public ICollection<Author> Authors { get; set; } = new List<Author>();
}
