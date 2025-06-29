using System;
using Microsoft.AspNetCore.Mvc;
using DemoBackend.Services;
using DemoBackend.Models.Authors;

namespace DemoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController(IAuthorService authorService) : ControllerBase, IAuthorController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            var author = await authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorRequestModel model)
        {
            var author = await authorService.CreateAuthorAsync(model);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] AuthorRequestModel model)
        {
            await authorService.UpdateAuthorAsync(id, model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            await authorService.DeleteAuthorAsync(id);
            return NoContent();
        }
    }
}
