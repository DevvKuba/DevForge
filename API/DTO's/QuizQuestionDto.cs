using System.Text.Json.Serialization;

namespace API.DTO_s
{
    public class QuizQuestionDto
    {
        public required string Type { get; set; }

        public required string Difficulty { get; set; }

        public required string Category { get; set; }

        public required string Question { get; set; }

        [JsonPropertyName("correct_answer")]
        public required string CorrectAnswer { get; set; }

        [JsonPropertyName("incorrect_answers")]
        public required List<string> IncorrectAnswers { get; set; }
    }
}
