namespace API.Entities
{
    public class Blog
    {
        public int Id { get; set; }

        public required string Description { get; set; }

        public DateTime PublishedAt { get; private set; }

        public bool IsDeleted { get; set; } = false;

        public int UserId { get; set; }

        public AppUser User { get; set; } = null!;

        public List<BlogLike> BlogLikes { get; set; } = [];

        public List<BlogComment> BlogComments { get; set; } = [];

    }
}
