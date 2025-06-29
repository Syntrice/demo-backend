using Microsoft.AspNetCore.Mvc;
using DemoBackend.Models.Books;

namespace DemoBackend.Controllers;

public interface IBookController
{
    Task<IActionResult> GetAllBooks();
    Task<IActionResult> GetBookById(Guid id);
    Task<IActionResult> CreateBook(BookRequestModel model);
    Task<IActionResult> UpdateBook(Guid id, BookRequestModel model);
    Task<IActionResult> DeleteBook(Guid id);
}
