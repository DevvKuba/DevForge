using System.ComponentModel.DataAnnotations;

namespace API.DTO_s
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username must be provided")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password must be provided")]
        public required string Password { get; set; }
    }
}
