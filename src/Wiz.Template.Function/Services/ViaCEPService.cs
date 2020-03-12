using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Wiz.Template.Function.Models;
using Wiz.Template.Function.Services.Interfaces;

namespace Wiz.Template.Function.Services
{
    public class ViaCEPService : IViaCEPService
    {
        private readonly HttpClient _httpClient;

        public ViaCEPService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ViaCEP> GetByCEPAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"{cep}/json");
            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ViaCEP>(stringResponse);
        }
    }
}