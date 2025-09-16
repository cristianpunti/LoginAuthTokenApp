using System.ComponentModel.DataAnnotations;

namespace LoginAuthToken.Client.Models
{
    public class IpConfig
    {
        [Required(ErrorMessage = "La IP es obligatoria")]
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$", ErrorMessage = "Formato de IP inválido")]
        public string Ip { get; set; } = string.Empty;
    }
}
