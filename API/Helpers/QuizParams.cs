namespace API.Helpers
{
    public class QuizParams : PaginationParams
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
    }
}
