using API.DTO_s;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz?> GetQuizByIdAsync(int id);

        Task<Quiz?> GetIncompleteUserQuizAsync(AppUser user);

        Task<PagedList<QuizDto>> GetUserQuizzesAsync(QuizParams quizParams);

        Task<bool> DoesUserHaveAnUnfinishedQuizAsync(AppUser user);

        Task SaveQuizAsync(Quiz quiz);

        void DeleteQuiz(Quiz quiz);
    }
}
