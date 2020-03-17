using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wiz.Template.Function;
using Wiz.Template.Function.Services;
using Wiz.Template.Integration.Tests.Configuration;
using Xunit;

namespace Wiz.Template.Integration.Tests.Functions.TimeTrigger
{
    public class ViaCEPTimerTriggerFunctionTest
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ViaCEPTimerTriggerFunctionTest()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
            _logger = FactoryConfiguration.CreateLogger();
        }

        [Fact]
        public async Task ViaCEPTimerTriggerFunction_ReturnLogMessageTest()
        {
            var logger = (ListLogger)FactoryConfiguration.CreateLogger(LoggerTypes.List);
            var service = new ViaCEPService(_httpClient);

            await new ViaCEPTimerTriggerFunction(service).Run(null, logger);

            var messageInit = logger.Logs[0];
            var messageService = logger.Logs[1];
            var messageFinished = logger.Logs[2];

            Assert.Contains("C# timer trigger function processed a request.", messageInit);
            Assert.Contains("{ CEP = 29315755, Street = Rua João Antônio Vasques, StreetFull = , UF = ES }", messageService);
            Assert.Contains("C# timer trigger function finished.", messageFinished);
        }
    }
}