namespace API.DTO_s
{
    public class BlogDto
    {
        public int Id { get; set; }

        public required string Description { get; set; }

        public DateTime PublishedAt { get; private set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int UserId { get; set; }
    }
}
