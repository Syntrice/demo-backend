using DemoBackend.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackend.Controllers;

public interface IBookController
{
    Task<IActionResult> GetAllBooks();
    Task<IActionResult> GetBookById(Guid id);
    Task<IActionResult> CreateBook(Book book);
    Task<IActionResult> UpdateBook(Guid id, Book book);
    Task<IActionResult> DeleteBook(Guid id);
}
