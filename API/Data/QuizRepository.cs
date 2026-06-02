using API.DTO_s;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Data
{
    public class QuizRepository(DataContext context, IMapper mapper) : IQuizRepository
    {
        public async Task<PagedList<QuizDto>> GetUserQuizzesAsync(QuizParams quizParams)
        {
            var query = context.Quizzes
                .OrderByDescending(q => q.CompletedAt)
                .Where(q => q.UserId == quizParams.UserId)
                .AsQueryable();

            var quizzes = query.ProjectTo<QuizDto>(mapper.ConfigurationProvider);

            return await PagedList<QuizDto>.CreateAsync(quizzes, quizParams.PageNumber, quizParams.PageSize);
        }

        // persist quiz once quser completes it

        // delete quiz - in case of not liking performance / accidental completion?
    }
}
