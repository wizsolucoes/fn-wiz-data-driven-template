using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wiz.Template.Auth;
using Wiz.Template.Function.Services.Interfaces;
using Wiz.Template.Function.Models;

namespace Wiz.Template.Function
{
    public class ViaCEPHttpTriggerFunction
    {
        private readonly IAccessTokenProvider _tokenProvider;
        private readonly IViaCEPService _viaCEPService;

        public ViaCEPHttpTriggerFunction(IAccessTokenProvider tokenProvider, IViaCEPService viaCEPService)
        {
            _tokenProvider = tokenProvider;
            _viaCEPService = viaCEPService;
        }

        [FunctionName("ViaCEPHttpTriggerFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var tokenResult = await _tokenProvider.ValidateToken(req);

            log.LogInformation($"Token result: {tokenResult.Status.ToString()}");
            log.LogInformation($"Request received for {tokenResult.Principal?.Identity.Name}.");

            string cep = req.Query["cep"];

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            cep = cep ?? data?.name;

            log.LogInformation($"CEP: {cep}");

            if (cep == null)
            {
                return new BadRequestObjectResult($"CEP required.");
            }

            var service = await _viaCEPService.GetByCEPAsync(cep);

            return new OkObjectResult(new
            {
                CEP = ViaCEP.CEPFormat(service.CEP),
                Street = service.Street,
                StreetFull = service.StreetFull,
                UF = service.UF
            });
        }
    }
}
