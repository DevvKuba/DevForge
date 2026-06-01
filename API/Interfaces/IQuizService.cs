using API.DTO_s;
using API.Helpers;

namespace API.Interfaces
{
    public interface IQuizService
    {
        Task<List<QuizDto>> CallOpenTriviaToRetrieveQuestions(int numberOfQuestions, Difficulties difficulty, QuestionTypes questionType);
    }
}
