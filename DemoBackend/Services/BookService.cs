using DemoBackend.Database;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Authors;
using DemoBackend.Models.Books;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Services;

public class BookService(ApplicationDbContext db) : IBookService
{
    public async Task<List<BookDetailsResponseModel>> GetAllBooksAsync()
    {
        return await db.Books.Include(e => e.Authors).Select(e => new BookDetailsResponseModel()
        {
            Id = e.Id.ToString(),
            Title = e.Title,
            Authors = e.Authors.Select(e => new AuthorResponseModel() { Name = e.Name, Id = e.Id.ToString() })
        }).ToListAsync();
    }

    public async Task<BookDetailsResponseModel?> GetBookByIdAsync(Guid id)
    {
        var entity = await db.Books.Include(e => e.Authors).FirstOrDefaultAsync(b => b.Id == id);
        if (entity == null) return null;
        return new BookDetailsResponseModel()
        {
            Id = entity.Id.ToString(),
            Title = entity.Title,
            Authors = entity.Authors.Select(e => new AuthorResponseModel() { Name = e.Name, Id = e.Id.ToString() })
        };
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        db.Books.Add(book);
        await db.SaveChangesAsync();
        return book;
    }

    public async Task UpdateBookAsync(Book book)
    {
        db.Books.Update(book);
        await db.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(Guid id)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book != null)
        {
            db.Books.Remove(book);
            await db.SaveChangesAsync();
        }
    }
}