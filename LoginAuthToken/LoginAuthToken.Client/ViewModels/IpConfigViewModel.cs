using System.ComponentModel.DataAnnotations;

namespace LoginAuthToken.Client.ViewModels
{
    public class IpConfigViewModel
    {
        [Required(ErrorMessage = "The IP is mandatory")]
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$", ErrorMessage = "Invalid IP format")]
        public string Ip { get; set; } = string.Empty;
    }
}
