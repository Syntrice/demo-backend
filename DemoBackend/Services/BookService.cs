using DemoBackend.Common.Mapping;
using DemoBackend.Common.Results;
using DemoBackend.Database;
using DemoBackend.Database.Entities;
using DemoBackend.Models.Books.Requests;
using DemoBackend.Models.Books.Responses;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Services;

public interface IBookService
{
    Task<Result<List<BookDetailsResponseModel>>> GetAllBooksAsync();
    Task<Result<BookDetailsResponseModel>> GetBookByIdAsync(Guid id);
    Task<Result<BookDetailsResponseModel>> CreateBookAsync(BookRequestModel model);
    Task<Result<Unit>> UpdateBookAsync(Guid id, BookRequestModel model);
    Task<Result<Unit>> DeleteBookAsync(Guid id);
    Task<Result<List<BookDetailsResponseModel>>> GetAllBooksByAuthorIdAsync(Guid authorId);
}

public class BookService(ApplicationDbContext db, IMapper mapper) : IBookService
{
    public async Task<Result<List<BookDetailsResponseModel>>> GetAllBooksAsync()
    {
        return await mapper.Map<Book, BookDetailsResponseModel>(db.Books.Include(e => e.Authors))
            .ToListAsync();
    }

    public async Task<Result<BookDetailsResponseModel>> GetBookByIdAsync(Guid id)
    {
        var entity = await mapper
            .Map<Book, BookDetailsResponseModel>(db.Books.Include(e => e.Authors))
            .FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null) return Error.NotFound($"Book with id '{id}' was not found.");
        return entity;
    }

    public async Task<Result<BookDetailsResponseModel>> CreateBookAsync(BookRequestModel model)
    {
        var requestedIds = model.AuthorIds.Distinct().ToList();

        var authors = await db.Authors
            .Where(a => requestedIds.Contains(a.Id))
            .ToListAsync();

        if (authors.Count != requestedIds.Count)
            return Error.Validation("One or more author IDs do not exist.");

        var book = new Book { Title = model.Title, Authors = authors };
        db.Books.Add(book);
        await db.SaveChangesAsync();
        return mapper.Map<Book, BookDetailsResponseModel>(book);
    }

    public async Task<Result<Unit>> UpdateBookAsync(Guid id, BookRequestModel model)
    {
        var book = await db.Books
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
            return Error.NotFound($"Book with id '{id}' was not found.");

        var requestedIds = model.AuthorIds.Distinct().ToList();

        var authors = await db.Authors
            .Where(a => requestedIds.Contains(a.Id))
            .ToListAsync();

        if (authors.Count != requestedIds.Count)
            return Error.Validation("One or more author IDs do not exist.");

        book.Title = model.Title;
        book.Authors = authors;
        await db.SaveChangesAsync();
        return Unit.Value;
    }

    public async Task<Result<Unit>> DeleteBookAsync(Guid id)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
            return Error.NotFound($"Book with id '{id}' was not found.");

        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return Unit.Value;
    }

    public async Task<Result<List<BookDetailsResponseModel>>> GetAllBooksByAuthorIdAsync(
        Guid authorId)
    {
        if (!await db.Authors.AnyAsync(a => a.Id == authorId))
            return Error.NotFound($"Author with id '{authorId}' was not found.");

        return await mapper
            .Map<Book, BookDetailsResponseModel>(db.Books.Include(b => b.Authors)
                .Where(b => b.Authors.Any(a => a.Id == authorId))).ToListAsync();
    }
}