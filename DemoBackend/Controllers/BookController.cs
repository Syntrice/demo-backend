using DemoBackend.Services;
using Microsoft.AspNetCore.Mvc;
using DemoBackend.Models.Books;

namespace DemoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService) : ControllerBase, IBookController
{
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookById(Guid id)
    {
        var book = await bookService.GetBookByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] BookRequestModel model)
    {
        var created = await bookService.CreateBookAsync(model);
        return CreatedAtAction(nameof(GetBookById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookRequestModel model)
    {
        await bookService.UpdateBookAsync(id, model);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        await bookService.DeleteBookAsync(id);
        return NoContent();
    }
}
