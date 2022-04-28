using System.Collections.Generic;
using System.Threading.Tasks;
using Correios_Services.Models;

namespace Correios_Services
{
    public interface ICorreiosService
    {
        Task<Package> GetPackageTrackingAsync(string packageCode);
        Package GetPackageTracking(string packageCode);
        
    }
}