using API.Entities;

namespace API.Interfaces
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetUserQuizzesAsync(AppUser user);
    }
}
