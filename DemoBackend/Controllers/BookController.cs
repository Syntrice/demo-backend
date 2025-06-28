using DemoBackend.Services;
using DemoBackend.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoBackend.Controllers;

[ApiController]
[Route("[controller]")]
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
    public async Task<IActionResult> CreateBook([FromBody] Book book)
    {
        var created = await bookService.CreateBookAsync(book);
        return CreatedAtAction(nameof(GetBookById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        await bookService.UpdateBookAsync(book);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        await bookService.DeleteBookAsync(id);
        return NoContent();
    }
}