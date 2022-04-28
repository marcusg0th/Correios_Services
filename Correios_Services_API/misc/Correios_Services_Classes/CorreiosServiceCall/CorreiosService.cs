using AngleSharp.Browser;
using Correios_Services.Extensions;
using Correios_Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Correios_Services
{
    public class CorreiosService : ICorreiosService
    {
        private const string PACKAGE_TRACKING_URL = "https://www.linkcorreios.com.br";

        private readonly HttpClient _httpClient;

        public CorreiosService()
        {
            _httpClient = new HttpClient();
        }

        #region Packages

        public async Task<Package> GetPackageTrackingAsync(string packageCode)
        {
            var url = $"{PACKAGE_TRACKING_URL}/?id={packageCode}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            var response = await _httpClient.SendAsync(requestMessage);
            var html = await response.Content.ReadAsStringAsync();
            return Parser.ParsePackage(html);
        }

        public Package GetPackageTracking(string packageCode)
        {
            return GetPackageTrackingAsync(packageCode).RunSync();
        }

        #endregion
        
    }
}
