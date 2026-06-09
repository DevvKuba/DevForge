using API.DTO_s;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz?> GetQuizByIdAsync(int id);
        Task<PagedList<QuizDto>> GetUserQuizzesAsync(QuizParams quizParams);

        Task SaveQuizAsync(Quiz quiz);

        void DeleteQuiz(Quiz quiz);
    }
}
