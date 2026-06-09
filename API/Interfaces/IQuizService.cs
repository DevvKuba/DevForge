using API.DTO_s;
using API.Helpers;

namespace API.Interfaces
{
    public interface IQuizService
    {
        Task<List<QuizQuestionDto>> RetrieveQuestionsAsync(int numberOfQuestions, string difficulty, string questionType);

        string RemoveSpecialCharacters(string input);
    }
}
