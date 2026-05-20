namespace API.DTO_s
{
    public class UserDto
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string KnownAs { get; set; }

        public required int AppExperiencePoints { get; set; } = 0;

        public required int Level { get; set; } = 1;

        public required int LevelThreshold { get; set; }

        public required string Token { get; set; }

        public required string Gender { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
