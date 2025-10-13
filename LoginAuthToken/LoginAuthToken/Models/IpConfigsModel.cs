using LoginAuthToken.Shared.Models;

namespace LoginAuthToken.Models
{
    public class IpConfigsModel
    {
        public List<IpConfigModel> Configs { get; set; } = new();
    }
}
