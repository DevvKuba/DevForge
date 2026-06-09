using API.DTO_s;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class QuizRepository(DataContext context, IMapper mapper) : IQuizRepository
    {
        public async Task<Quiz?> GetQuizByIdAsync(int id)
        {
            var quiz = await context.Quizzes.Where(q => q.Id == id).FirstOrDefaultAsync();
            return quiz;
        }
        public async Task<PagedList<QuizDto>> GetUserQuizzesAsync(QuizParams quizParams)
        {
            var query = context.Quizzes
                .OrderByDescending(q => q.CompletedAt)
                .Where(q => q.UserId == quizParams.UserId)
                .AsQueryable();

            var quizzes = query.ProjectTo<QuizDto>(mapper.ConfigurationProvider);

            return await PagedList<QuizDto>.CreateAsync(quizzes, quizParams.PageNumber, quizParams.PageSize);
        }

        public async Task SaveQuizAsync(Quiz quiz)
        {
            await context.Quizzes.AddAsync(quiz);
        }

        public void DeleteQuiz(Quiz quiz)
        {
            context.Quizzes.Remove(quiz);
        }
    }
}
