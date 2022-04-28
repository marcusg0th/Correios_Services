using Correios_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace Correios_Services_API.Controlls
{
    [ApiController]
    [Route("encomendas")]
    public class PackageTrackingController : ControllerBase
    {
        private readonly ICorreiosService _services;
        private readonly ILogger<PackageTrackingController> _logger;

        public PackageTrackingController(ICorreiosService services, ILogger<PackageTrackingController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [Route("{code}")]
        [HttpGet]
        public async Task<Correios_Services.Models.Package> Get(string code)
        {
            var package = await _services.GetPackageTrackingAsync(code);
            return package;

        }
    }
}
