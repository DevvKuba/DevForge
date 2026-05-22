using API.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.DTO_s
{
    public class BlogDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Blog title must be provided")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Blog description must be provided")]
        public required string Description { get; set; }

        public DateTime PublishedAt { get; private set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int UserId { get; set; }

        public int? InteractingUserId { get; set; }

        public List<BlogLikeDto> BlogLikes { get; set; } = [];

        public List<BlogCommentDto> BlogComments { get; set; } = [];
    }
}
