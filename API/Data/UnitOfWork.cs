using API.Interfaces;

namespace API.Data
{
    // A scoped service
    public class UnitOfWork(DataContext context, IUserRepository userRepository, ILikesRepository likesRepository,
        IMessageRepository messageRepository, IPhotoRepository photoRepository, IBlogRepository blogRepository,
        IBlogLikeRepository blogLikeRepository, IBlogCommentRepository blogCommentRepository, IQuizRepository quizRepository) : IUnitOfWork
    {
        public IUserRepository UserRepository => userRepository;

        public IMessageRepository MessageRepository => messageRepository;

        public ILikesRepository LikesRepository => likesRepository;

        public IPhotoRepository PhotoRepository => photoRepository;

        public IBlogRepository BlogRepository => blogRepository;

        public IBlogLikeRepository BlogLikeRepository => blogLikeRepository;

        public IBlogCommentRepository BlogCommentRepository => blogCommentRepository;

        public IQuizRepository QuizRepository => quizRepository;

        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }
    }
}
