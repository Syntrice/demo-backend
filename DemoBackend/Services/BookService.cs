using DemoBackend.Database;
using DemoBackend.Models.Authors;
using DemoBackend.Models.Authors.Responses;
using DemoBackend.Models.Books;
using DemoBackend.Models.Books.Requests;
using DemoBackend.Models.Books.Responses;
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

    public async Task<BookDetailsResponseModel> CreateBookAsync(BookRequestModel model)
    {
        var authors = await db.Authors
            .Where(a => model.AuthorIds.Contains(a.Id))
            .ToListAsync();

        var book = new Database.Entities.Book { Title = model.Title, Authors = authors };
        db.Books.Add(book);
        await db.SaveChangesAsync();
        return new BookDetailsResponseModel
        {
            Id = book.Id.ToString(),
            Title = book.Title,
            Authors = authors.Select(a => new AuthorResponseModel
            {
                Id = a.Id.ToString(),
                Name = a.Name
            })
        };
    }

    public async Task UpdateBookAsync(Guid id, BookRequestModel model)
    {
        var book = await db.Books
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (book is null) return;

        book.Title = model.Title;
        book.Authors = await db.Authors
            .Where(a => model.AuthorIds.Contains(a.Id))
            .ToListAsync();
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

    public async Task<List<BookDetailsResponseModel>> GetAllBooksByAuthorIdAsync(Guid authorId)
    {
        return await db.Books.Include(b => b.Authors).Where(b => b.Authors.Any(a => a.Id == authorId))
            .Select(b => new BookDetailsResponseModel()
            {
                Id = b.Id.ToString(),
                Title = b.Title,
                Authors = b.Authors.Select(e => new AuthorResponseModel() { Name = e.Name, Id = e.Id.ToString() })
            }).ToListAsync();
    }
}