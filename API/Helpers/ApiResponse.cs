using API.DTO_s;

namespace API.Helpers
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }

        public string? Message { get; set; }

        public bool Success { get; set; }

        public required UserXpDetailDto XpDetails { get; set; }

    }
}
