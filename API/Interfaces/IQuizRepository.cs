using API.DTO_s;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IQuizRepository
    {
        Task<PagedList<QuizDto>> GetUserQuizzesAsync(QuizParams quizParams);
    }
}
