using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestAPIs.Services;

namespace TestAPIs.HttpClients
{
    public class WooliesXClient : IWooliesXClient
    {
        private readonly ISettingService _settingService;
        private readonly ILogger<WooliesXClient> _logger;

        public WooliesXClient(ISettingService settingService, ILogger<WooliesXClient> logger)
        {
            _settingService = settingService;
            _logger = logger;
        }

        private HttpClient _httpClient;
        private HttpResponseMessage Response { get; set; }

        public async Task<T> GetAsync<T>(string requestUrl) where T : new()
        {
            InitHttpClient();
            var url = $"{requestUrl}";
            _logger.LogDebug($"Http Get request on {_httpClient.BaseAddress}{requestUrl}");
            Response = await _httpClient.GetAsync(AppendTokenInURL(requestUrl));
            return await ReadResponse<T>();
        }

        public async Task<T> PostAsync<T>(string requestUrl, object body = null) where T : new()
        {
            InitHttpClient();
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(body, settings);
            _logger.LogDebug($"Http Post request on {_httpClient.BaseAddress}{requestUrl}");
            Response = await _httpClient.PostAsync(AppendTokenInURL(requestUrl), new StringContent(json, Encoding.UTF8, "application/json"));
            return await ReadResponse<T>();
        }

        private void InitHttpClient()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_settingService.WooliesXBaseUrl),
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
        }

        private async Task<T> ReadResponse<T>() where T : new()
        {
            if (Response.IsSuccessStatusCode)
            {
                var result = await Response.Content.ReadAsAsync<T>();
                return result;
            }

            var errorMessage = $"Could not get successful response from URI:{Response.RequestMessage.RequestUri}, Method:{Response.RequestMessage.Method}, Status:{Response.StatusCode.ToString()}.";
            _logger.LogError(errorMessage);

            throw new Exception(errorMessage);
        }

        private string AppendTokenInURL(string requestUrl)
        {
            if (requestUrl.Contains("?"))
            {
                return $"{requestUrl}&token={_settingService.WooliesXUser.Token}";
            }
            else
            {
                return $"{requestUrl}?token={_settingService.WooliesXUser.Token}";
            }
        }
    }
}
