using System;

namespace DemoBackend.Database.Entities
{
    public class Author : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
