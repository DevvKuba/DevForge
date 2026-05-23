using System.ComponentModel.DataAnnotations;

namespace API.DTO_s
{
    public class BlogCommentDto
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime UpdatedAt { get; set; }

        [Required(ErrorMessage = "Must provide content to post comment")]
        public required string Content { get; set; }

        public int BlogId { get; set; }
    }
}
