using Microsoft.AspNetCore.Mvc;
using LoginAuthToken.Server.Services;
using LoginAuthToken.Client.Models;

namespace LoginAuthToken.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IpConfigController : ControllerBase
    {
        private readonly IpConfigService _service;
        private readonly ILogger<IpConfigController> _logger;

        public IpConfigController(IpConfigService service, ILogger<IpConfigController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // POST: api/IpConfig/save
        [HttpPost("save")]
        public IActionResult SaveIp([FromBody] IpConfig input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Ip))
                return BadRequest(new { Success = false });

            // Guardar en session
            HttpContext.Session.SetString("ClientIp", input.Ip);
            _logger.LogInformation("IP guardada en session: {Ip}", input.Ip);

            return Ok(new { Success = true });
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var ips = _service.GetAllIps();
                _logger.LogInformation("Se enviaron {Count} IPs al cliente", ips.Count);
                return Ok(ips);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GET api/IpConfig");
                return StatusCode(500, "Error al leer las IPs del servidor.");
            }
        }

        // POST: api/IpConfig/validate
        [HttpPost("validate")]
        public IActionResult ValidateIp([FromBody] IpConfig input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Ip))
                return BadRequest(new { Valid = false });

            var ips = _service.GetAllIps();
            bool valid = ips.Any(x => x == input.Ip);

            return Ok(new { Valid = valid });
        }

        [HttpGet("current")]
        public IActionResult GetCurrentIp()
        {
            var ip = HttpContext.Session.GetString("ClientIp");
            if (string.IsNullOrEmpty(ip))
            {
                return Ok(new { Ip = "" });
            }

            return Ok(new { Ip = ip });
        }

    }
}
