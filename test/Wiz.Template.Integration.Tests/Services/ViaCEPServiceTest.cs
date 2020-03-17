using System;
using System.Net.Http;
using System.Threading.Tasks;
using Wiz.Template.Function.Models;
using Wiz.Template.Function.Services;
using Xunit;

namespace Wiz.Template.Integration.Tests.Services
{
    public class ViaCEPServiceTest
    {
        private readonly HttpClient _httpClient;

        public ViaCEPServiceTest()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
        }

        [Theory]
        [InlineData("68901111")]
        [InlineData("64010445")]
        [InlineData("44052061")]
        [InlineData("68906810")]
        public async Task GetByCEPAsync_ReturnViaCEPModelTest(string cep)
        {
            var service = new ViaCEPService(_httpClient);
            var method = await service.GetByCEPAsync(cep);

            var result = Assert.IsType<ViaCEP>(method);

            Assert.NotNull(result);
            Assert.NotNull(result.CEP);
        }
    }
}