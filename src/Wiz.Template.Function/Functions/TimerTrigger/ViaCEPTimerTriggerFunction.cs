using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Wiz.Template.Function.Services.Interfaces;

namespace Wiz.Template.Function
{
    public class ViaCEPTimerTriggerFunction
    {
        private readonly IViaCEPService _viaCEPService;

        public ViaCEPTimerTriggerFunction(IViaCEPService viaCEPService)
        {
            _viaCEPService = viaCEPService;
        }

        [FunctionName("ViaCEPTimerTriggerFunction")]
        public async Task Run(
            [TimerTrigger("0 */1 * * * *")]TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation("C# timer trigger function processed a request.");

            var service = await _viaCEPService.GetByCEPAsync(cep: "29315755");

            var responseMessage = new
            {
                CEP = service.CEP,
                Street = service.Street,
                StreetFull = service.StreetFull,
                UF = service.UF
            };

            log.LogInformation($"{responseMessage}");

            log.LogInformation($"C# timer trigger function finished {DateTime.Now}.");
        }
    }
}