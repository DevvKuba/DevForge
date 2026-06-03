using API.Helpers;

namespace API.Entities
{
    public class Quiz
    {
        public int Id { get; set; }

        public required string Difficulty { get; set; }

        public required List<QuizQuestion> Questions { get; set; }

        public double PercentageScore { get; set; }

        public int XpAwardedForCompletion { get; set; }

        public DateTime CompletedAt { get; set; }

        public int UserId { get; set; }

        public AppUser User { get; set; } = null!;

    }
}
