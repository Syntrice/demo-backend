using DemoBackend.Models.Books;

namespace DemoBackend.Services;

public interface IBookService
{
    Task<List<BookDetailsResponseModel>> GetAllBooksAsync();
    Task<BookDetailsResponseModel?> GetBookByIdAsync(Guid id);
    Task<BookDetailsResponseModel> CreateBookAsync(BookRequestModel model);
    Task UpdateBookAsync(Guid id, BookRequestModel model);
    Task DeleteBookAsync(Guid id);
}
