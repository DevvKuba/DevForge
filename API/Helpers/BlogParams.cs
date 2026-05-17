namespace API.Helpers
{
    public class BlogParams : PaginationParams
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string? TitleSearchTerm { get; set; }

    }
}
