using System.ComponentModel.DataAnnotations;

namespace LoginAuthToken.Client.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required")]
      
        public string Password { get; set; } = string.Empty;
   
         public class IpConfig
    {
        public string Ip { get; set; } = string.Empty;
    }
    }
}
