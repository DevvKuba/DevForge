using API.Entities;
using API.Helpers;

namespace API.DTO_s
{
    public class QuizDto
    {
        public int? Id { get; set; }
        public required string Difficulty { get; set; } 

        public required List<QuizQuestionDto> Questions { get; set; }

        public double PercentageScore { get; set; }

        public bool IsComplete { get; set; }

        public int UserId { get; set; }
    }
}
