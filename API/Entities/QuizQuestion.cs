namespace API.Entities
{
    public class QuizQuestion
    {
        public int Id { get; set; }

        public required string Question { get; set; }

        public required string CorrectAnswer { get; set; }

        public required List<string> IncorrectAnswers { get; set; }

        public required string Difficulty { get; set; }

        public int QuizId { get; set; }

        public Quiz Quiz { get; set; } = null!;
    }
}
