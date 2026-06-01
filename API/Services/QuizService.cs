using API.DTO_s;
using API.Helpers;
using API.Interfaces;

namespace API.Services
{
    public class QuizService(IUnitOfWork unitOfWork) : IQuizService
    {
        public Task<List<QuizDto>> CallOpenTriviaToRetrieveQuestions(int numberOfQuestions, Difficulties difficulty, QuestionTypes questionType)
        {
            throw new NotImplementedException();
        }
    }
}
