using System.ComponentModel.DataAnnotations;

namespace AppsGenerator.Models
{
    public class ManageUserViewModel
    {
        [Required(ErrorMessage="Enter current password")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Enter New Password")]
        [DataType(DataType.Password)]
        [StringLength(18, ErrorMessage = "Password must be between 6-18 character", MinimumLength = 6)]
        [RegularExpression(@"^[A-Za-z]+[_A-Za-z\d]+$", ErrorMessage = "Enter a strong password, you must start with a letter and that it contains letters, numbers, or _, letters English only")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Retype New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage="Enter Username")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage="Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }

    public class SignupViewModel
    {

        [Required(ErrorMessage = "Enter Username")]
        [Display(Name = "Username")]
        [StringLength(16, ErrorMessage = "The Username must be between 4 to 16", MinimumLength = 4)]
        [RegularExpression(@"^[A-Za-z]+[_A-Za-z\d]+$", ErrorMessage = "Must begin with a letter and that it contains letters, numbers, or _, letters English only")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Email Address")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "E-mail format is incorrect")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        [StringLength(18, ErrorMessage = "Password must be between 6-18 character", MinimumLength = 6)]
        [RegularExpression(@"^[A-Za-z]+[_A-Za-z\d]+$", ErrorMessage = "Enter a strong password, you must start with a letter and that it contains letters, numbers, or _, letters English only")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Retype Password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordRequestViewModel
    {
        [Required(ErrorMessage = "Enter Email Address")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "E-mail format is incorrect")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        [StringLength(18, ErrorMessage = "Password must be between 6-18 character", MinimumLength = 6)]
        [RegularExpression(@"^[A-Za-z]+[_A-Za-z\d]+$", ErrorMessage = "Enter a strong password, you must start with a letter and that it contains letters, numbers, or _, letters English only")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Retype Password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string email { get; set; }
        [Required]
        public string token { get; set; }
    }
}
