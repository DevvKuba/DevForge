using API.Helpers;

namespace API.Entities
{
    public class Quiz
    {
        public int Id { get; set; }

        public Difficulties Difficulty { get; set; }

        public required List<QuizQuestion> Questions { get; set; }

        public int Score { get; set; }

        public DateTime CompletedAt { get; set; }

        public int UserId { get; set; }

        public AppUser User { get; set; } = null!;

    }
}
