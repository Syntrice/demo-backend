using DemoBackend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace DemoBackend.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(Guid id);
    Task<Book> CreateBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(Guid id);
}
