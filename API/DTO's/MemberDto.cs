namespace API.DTO_s
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }

        public int Age { get; set; }

        public int YearsOfExperience { get; set; }

        public string? Specialization { get; set; }

        public string? Email { get; set; }

        public string? PhotoUrl { get; set; }

        public string? KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string? Gender { get; set; }

        public string? Introduction { get; set; }

        public string? Interests { get; set; }

        public int AppExperiencePoints { get; set; }

        public int Level { get; set; }

        public int LevelThreshold { get; set; }

        // refactored
        public string? Skills { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public List<PhotoDto>? Photos { get; set; }

    }
}
