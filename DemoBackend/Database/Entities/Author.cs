using System;
using System.Collections;

namespace DemoBackend.Database.Entities
{
    public class Author : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}