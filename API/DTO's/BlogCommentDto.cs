namespace API.DTO_s
{
    public class BlogCommentDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime UpdatedAt { get; set; }

        public required string Content { get; set; }

        public int BlogId { get; set; }
    }
}
