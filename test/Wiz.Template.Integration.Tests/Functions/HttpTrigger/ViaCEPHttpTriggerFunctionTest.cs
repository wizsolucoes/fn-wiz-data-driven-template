using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wiz.Template.Auth;
using Wiz.Template.Function;
using Wiz.Template.Function.Services;
using Wiz.Template.Integration.Tests.Configuration;
using Xunit;

namespace Wiz.Template.Integration.Tests.Functions.HttpTrigger
{
    public class ViaCEPHttpTriggerFunctionTest
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ViaCEPHttpTriggerFunctionTest()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
            _logger = FactoryConfiguration.CreateLogger();
        }

        [Theory]
        [InlineData("cep", "68901111")]
        [InlineData("cep", "64010445")]
        [InlineData("cep", "44052061")]
        [InlineData("cep", "68906810")]
        public async Task ViaCEPHttpTriggerFunction_NoAuthReturnViaCEPTest(string query, string cep)
        {
            var service = new ViaCEPService(_httpClient);
            var accessToken = new AccessTokenProvider(string.Empty, string.Empty);

            var request = FactoryConfiguration.CreateHttpRequest(query, cep);
            var response = (OkObjectResult)await new ViaCEPHttpTriggerFunction(accessToken, service).Run(request, _logger);

            dynamic data = response.Value;

            Assert.NotNull(data);
        }
    }
}