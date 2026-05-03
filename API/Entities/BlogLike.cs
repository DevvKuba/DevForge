using System.Reflection.Metadata.Ecma335;

namespace API.Entities
{
    public class BlogLike
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime UpdatedAt { get; set; }

        public int BlogId { get; set; }

        public Blog Blog { get; set; } = null!;
    }
}
