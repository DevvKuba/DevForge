using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace API.Entities;

// User Schema for the database
public class AppUser : IdentityUser<int>
{
    public DateOnly DateOfBirth { get; set; }

    public required string KnownAs { get; set; }

    public int YearsOfExperience { get; set; }

    public string? Specialization { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime LastActive { get; set; } = DateTime.UtcNow;

    public required string Gender { get; set; }

    public string? Introduction { get; set; }

    public string? Interests { get; set; }

    public string? Skills { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public List<Photo> Photos { get; set; } = [];

    public List<UserLike> LikedByUsers { get; set; } = [];

    public List<UserLike> LikedUsers { get; set; } = [];

    public List<Message> MessagesSent { get; set; } = [];

    public List<Message> MessagesRecieved { get; set; } = [];

    public List<Blog> Blogs { get; set; } = [];

    // join 
    public ICollection<AppUserRole> UserRoles { get; set; } = [];



}
