namespace LoginAuthToken.Server.Models
{
    public class IpConfig
    {
        public string Ip { get; set; } = default!;
    }

    public class IpConfigs
    {
        public List<IpConfig> Configs { get; set; } = new();
    }
}