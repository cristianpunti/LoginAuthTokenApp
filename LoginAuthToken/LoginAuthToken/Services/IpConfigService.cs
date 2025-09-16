using System.Xml.Linq;

namespace LoginAuthToken.Server.Services
{
    public class IpConfigService
    {
        private readonly string _filePath;

        public IpConfigService(string filePath)
        {
            _filePath = filePath;
        }

        public List<string> GetAllIps()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("No se encontró el archivo XML", _filePath);

            var doc = XDocument.Load(_filePath);

            return doc.Descendants("IpConfig")
                      .Select(x => x.Element("Ip")?.Value.Trim())
                      .Where(x => !string.IsNullOrEmpty(x))
                      .ToList()!;
        }
    }
}
