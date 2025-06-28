using System;
using Microsoft.AspNetCore.Mvc;
using DemoBackend.Services;
using DemoBackend.Database.Entities;

namespace DemoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController(IAuthorService authorService) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            return Ok(authorService.GetAllAuthors());
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthorById(Guid id)
        {
            var author = authorService.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost]
        public IActionResult AddAuthor(Author author)
        {
            authorService.AddAuthor(author);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(Guid id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            authorService.UpdateAuthor(author);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(Guid id)
        {
            authorService.DeleteAuthor(id);
            return NoContent();
        }
    }
}
