using System.ComponentModel.DataAnnotations;

namespace Authentication.DomainModels
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[a-zA-Z0-9.a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$#!%*?&_])[A-Za-z\d@$#!%*?&_].{7,}$",
            ErrorMessage = "Password must be at least 8 characters long and include at least one lowercase letter, one uppercase letter, one digit, and one special character (@$#!%*?&_).")]

        public string Password { get; set; }
    }

}
