namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IMessageRepository MessageRepository { get; }

        ILikesRepository LikesRepository { get; }

        IPhotoRepository PhotoRepository { get; }

        IBlogRepository BlogRepository { get; }

        IBlogLikeRepository BlogLikeRepository { get; }

        IBlogCommentRepository BlogCommentRepository { get; }

        IQuizRepository QuizRepository { get; }

        Task<bool> Complete();

        bool HasChanges();
    }
}
