using System.ComponentModel.DataAnnotations;

namespace API.DTO_s
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username must be provided")] 
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Known as property must be provided")]
        public string? KnownAs { get; set; }

        [Required(ErrorMessage = "Gender must be provided")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Date of birth must be provided")]
        public string? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Specializaion must be provided")]
        public string? Specialization { get; set; }

        [Required(ErrorMessage = "Years of experience must be provided")]
        public int? yearsOfExperience { get; set; }


        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password needs to be at least 8 characters")]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must contain uppercase, lowercase, number, and special character.")]
        public string Password { get; set; } = string.Empty;
    }
}
