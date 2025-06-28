using DemoBackend.Database;
using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Services;

public class BookService(ApplicationDbContext db) : IBookService
{
    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await db.Books.ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(Guid id)
    {
        return await db.Books.FirstOrDefaultAsync(b => b.Id == id);
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