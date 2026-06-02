namespace API.DTO_s
{
    public class QuizInfoDto
    {
        public required int NumberOfQuestions { get; set; }

        public required string Difficulty { get; set; }

        public required string QuestionType { get; set; }


    }
}
