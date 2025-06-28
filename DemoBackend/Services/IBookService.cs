using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DemoBackend.Models.Books;

namespace DemoBackend.Services;

public interface IBookService
{
    Task<List<BookDetailsResponseModel>> GetAllBooksAsync();
    Task<BookDetailsResponseModel?> GetBookByIdAsync(Guid id);
    Task<Book> CreateBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(Guid id);
}
