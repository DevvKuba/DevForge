namespace API.Entities
{
    public class Quiz
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public required string Topic { get; set; } // change to enum

        public required string Difficulty { get; set; } // change to enum

        public required List<QuizQuestion> Questions { get; set; }

        public int Score { get; set; }

        public DateTime CompletedAt { get; set; }

    }
}
