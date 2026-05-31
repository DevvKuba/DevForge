using API.Entities;
using API.Helpers;

namespace API.DTO_s
{
    public class QuizDto
    {
        public int Id { get; set; }

        public Difficulties Difficulty { get; set; } 

        public required List<QuizQuestionDto> Questions { get; set; }

        public int Score { get; set; }

        public DateTime CompletedAt { get; set; }

        public int UserId { get; set; }
    }
}
