namespace DemoBackend.Database.Entities;

public class Book : IEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
}