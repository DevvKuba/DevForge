namespace API.Helpers
{
    public class BlogParams : PaginationParams
    {
        public int UserId { get; set; }

        // something to determine the blogs that they are interested in ? 
        // categories of sorts potentially 
    }
}
